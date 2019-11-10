using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class DeviceStatsController : Controller
    {
        readonly DicServerContext _context;

        public DeviceStatsController(DicServerContext context) => _context = context;

        // GET: Admin/DeviceStats
        public async Task<IActionResult> Index() =>
            View(await _context.DeviceStats.Include(d => d.Report).OrderBy(d => d.Manufacturer).ThenBy(d => d.Model).
                                ThenBy(d => d.Bus).ToListAsync());

        // GET: Admin/DeviceStats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            DeviceStat deviceStat = await _context.DeviceStats.FindAsync(id);

            if(deviceStat == null)
            {
                return NotFound();
            }

            return View(deviceStat);
        }

        // POST: Admin/DeviceStats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Manufacturer,Model,Revision,Bus")]
                                              DeviceStat deviceStat)
        {
            if(id != deviceStat.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(deviceStat);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!DeviceStatExists(deviceStat.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(deviceStat);
        }

        // GET: Admin/DeviceStats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            DeviceStat deviceStat = await _context.DeviceStats.FirstOrDefaultAsync(m => m.Id == id);

            if(deviceStat == null)
            {
                return NotFound();
            }

            return View(deviceStat);
        }

        // POST: Admin/DeviceStats/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            DeviceStat deviceStat = await _context.DeviceStats.FindAsync(id);
            _context.DeviceStats.Remove(deviceStat);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool DeviceStatExists(int id) => _context.DeviceStats.Any(e => e.Id == id);
    }
}