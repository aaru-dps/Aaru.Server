using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class DevicesController : Controller
    {
        readonly DicServerContext _context;

        public DevicesController(DicServerContext context) => _context = context;

        // GET: Admin/Devices
        public async Task<IActionResult> Index() => View(await _context.Devices.ToListAsync());

        // GET: Admin/Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Device device = await _context.Devices.FirstOrDefaultAsync(m => m.Id == id);

            if(device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // GET: Admin/Devices/Create
        public IActionResult Create() => View();

        // POST: Admin/Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("AddedWhen,ModifiedWhen,OptimalMultipleSectorsRead,Id,CompactFlash,Manufacturer,Model,Revision,Type")]
            Device device)
        {
            if(ModelState.IsValid)
            {
                _context.Add(device);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(device);
        }

        // GET: Admin/Devices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Device device = await _context.Devices.FindAsync(id);

            if(device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Admin/Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind(
                "AddedWhen,ModifiedWhen,OptimalMultipleSectorsRead,Id,CompactFlash,Manufacturer,Model,Revision,Type")]
            Device device)
        {
            if(id != device.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(device);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!DeviceExists(device.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(device);
        }

        // GET: Admin/Devices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Device device = await _context.Devices.FirstOrDefaultAsync(m => m.Id == id);

            if(device == null)
            {
                return NotFound();
            }

            return View(device);
        }

        // POST: Admin/Devices/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Device device = await _context.Devices.FindAsync(id);
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool DeviceExists(int id) => _context.Devices.Any(e => e.Id == id);
    }
}