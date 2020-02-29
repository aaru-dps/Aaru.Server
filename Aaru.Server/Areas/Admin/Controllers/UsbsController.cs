using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aaru.Server.Models;
using DiscImageChef.CommonTypes.Metadata;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Aaru.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class UsbsController : Controller
    {
        readonly DicServerContext _context;

        public UsbsController(DicServerContext context) => _context = context;

        // GET: Admin/Usbs
        public async Task<IActionResult> Index() =>
            View(await _context.Usb.OrderBy(u => u.Manufacturer).ThenBy(u => u.Product).ThenBy(u => u.VendorID).
                                ThenBy(u => u.ProductID).ToListAsync());

        // GET: Admin/Usbs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Usb usb = await _context.Usb.FirstOrDefaultAsync(m => m.Id == id);

            if(usb == null)
            {
                return NotFound();
            }

            return View(usb);
        }

        // GET: Admin/Usbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Usb usb = await _context.Usb.FirstOrDefaultAsync(m => m.Id == id);

            if(usb == null)
            {
                return NotFound();
            }

            return View(usb);
        }

        // POST: Admin/Usbs/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Usb usb = await _context.Usb.FindAsync(id);
            _context.Usb.Remove(usb);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Consolidate()
        {
            List<UsbModel> dups = _context.Usb.GroupBy(x => new
            {
                x.Manufacturer, x.Product, x.VendorID, x.ProductID
            }).Where(x => x.Count() > 1).Select(x => new UsbModel
            {
                Manufacturer = x.Key.Manufacturer, Product = x.Key.Product, VendorID = x.Key.VendorID,
                ProductID    = x.Key.ProductID
            }).ToList();

            return View(new UsbModelForView
            {
                List = dups, Json = JsonConvert.SerializeObject(dups)
            });
        }

        [HttpPost, ActionName("Consolidate"), ValidateAntiForgeryToken]
        public IActionResult ConsolidateConfirmed(string models)
        {
            UsbModel[] duplicates;

            try
            {
                duplicates = JsonConvert.DeserializeObject<UsbModel[]>(models);
            }
            catch(JsonSerializationException)
            {
                return BadRequest();
            }

            if(duplicates is null)
                return BadRequest();

            foreach(UsbModel duplicate in duplicates)
            {
                Usb master = _context.Usb.FirstOrDefault(m => m.Manufacturer == duplicate.Manufacturer &&
                                                              m.Product      == duplicate.Product      &&
                                                              m.VendorID     == duplicate.VendorID     &&
                                                              m.ProductID    == duplicate.ProductID);

                if(master is null)
                    continue;

                foreach(Usb slave in _context.Usb.Where(m => m.Manufacturer == duplicate.Manufacturer &&
                                                             m.Product      == duplicate.Product      &&
                                                             m.VendorID     == duplicate.VendorID     &&
                                                             m.ProductID    == duplicate.ProductID).Skip(1).ToArray())
                {
                    if(slave.Descriptors  != null &&
                       master.Descriptors != null)
                    {
                        if(!master.Descriptors.SequenceEqual(slave.Descriptors))
                            continue;
                    }

                    foreach(Device device in _context.Devices.Where(d => d.USB.Id == slave.Id))
                    {
                        device.USB = master;
                    }

                    foreach(UploadedReport report in _context.Reports.Where(d => d.USB.Id == slave.Id))
                    {
                        report.USB = master;
                    }

                    if(master.Descriptors is null &&
                       slave.Descriptors != null)
                    {
                        master.Descriptors = slave.Descriptors;
                        _context.Usb.Update(master);
                    }

                    _context.Usb.Remove(slave);
                }
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}