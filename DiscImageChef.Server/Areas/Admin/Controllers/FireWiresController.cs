using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class FireWiresController : Controller
    {
        readonly DicServerContext _context;

        public FireWiresController(DicServerContext context) => _context = context;

        // GET: Admin/FireWires
        public async Task<IActionResult> Index() =>
            View(await _context.FireWire.OrderBy(f => f.Manufacturer).ThenBy(f => f.Product).ToListAsync());

        // GET: Admin/FireWires/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            FireWire fireWire = await _context.FireWire.FindAsync(id);

            if(fireWire == null)
            {
                return NotFound();
            }

            return View(fireWire);
        }

        // POST: Admin/FireWires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("Id,VendorID,ProductID,Manufacturer,Product,RemovableMedia")]
            FireWire fireWire)
        {
            if(id != fireWire.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(fireWire);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!FireWireExists(fireWire.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(fireWire);
        }

        // GET: Admin/FireWires/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            FireWire fireWire = await _context.FireWire.FirstOrDefaultAsync(m => m.Id == id);

            if(fireWire == null)
            {
                return NotFound();
            }

            return View(fireWire);
        }

        // POST: Admin/FireWires/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            FireWire fireWire = await _context.FireWire.FindAsync(id);
            _context.FireWire.Remove(fireWire);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool FireWireExists(int id) => _context.FireWire.Any(e => e.Id == id);

        public IActionResult Consolidate()
        {
            List<FireWireModel> dups = _context.FireWire.GroupBy(x => new
            {
                x.VendorID, x.ProductID, x.Manufacturer, x.Product,
                x.RemovableMedia
            }).Where(x => x.Count() > 1).Select(x => new FireWireModel
            {
                VendorID = x.Key.VendorID, ProductID     = x.Key.ProductID, Manufacturer = x.Key.Manufacturer,
                Product  = x.Key.Product, RemovableMedia = x.Key.RemovableMedia
            }).ToList();

            return View(new FireWireModelForView
            {
                List = dups, Json = JsonConvert.SerializeObject(dups)
            });
        }

        [HttpPost, ActionName("Consolidate"), ValidateAntiForgeryToken]
        public IActionResult ConsolidateConfirmed(string models)
        {
            FireWireModel[] duplicates;

            try
            {
                duplicates = JsonConvert.DeserializeObject<FireWireModel[]>(models);
            }
            catch(JsonSerializationException)
            {
                return BadRequest();
            }

            if(duplicates is null)
                return BadRequest();

            foreach(FireWireModel duplicate in duplicates)
            {
                FireWire master = _context.FireWire.FirstOrDefault(m => m.VendorID       == duplicate.VendorID     &&
                                                                        m.ProductID      == duplicate.ProductID    &&
                                                                        m.Manufacturer   == duplicate.Manufacturer &&
                                                                        m.Product        == duplicate.Product      &&
                                                                        m.RemovableMedia == duplicate.RemovableMedia);

                if(master is null)
                    continue;

                foreach(FireWire firewire in _context.FireWire.Where(m => m.VendorID       == duplicate.VendorID     &&
                                                                          m.ProductID      == duplicate.ProductID    &&
                                                                          m.Manufacturer   == duplicate.Manufacturer &&
                                                                          m.Product        == duplicate.Product      &&
                                                                          m.RemovableMedia == duplicate.RemovableMedia).
                                                      Skip(1).ToArray())
                {
                    foreach(Device device in _context.Devices.Where(d => d.FireWire.Id == firewire.Id))
                    {
                        device.FireWire = master;
                    }

                    foreach(UploadedReport report in _context.Reports.Where(d => d.FireWire.Id == firewire.Id))
                    {
                        report.FireWire = master;
                    }

                    _context.FireWire.Remove(firewire);
                }
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}