using System;
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
        public async Task<IActionResult> Index() =>
            View(await _context.Devices.OrderBy(d => d.Manufacturer).ThenBy(d => d.Model).ThenBy(d => d.Revision).
                                ThenBy(d => d.CompactFlash).ThenBy(d => d.Type).ToListAsync());

        // GET: Admin/Devices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var model = new DeviceDetails
            {
                Report = await _context.Devices.Include(d => d.ATA).Include(d => d.ATAPI).Include(d => d.SCSI).
                                        Include(d => d.MultiMediaCard).Include(d => d.SecureDigital).
                                        Include(d => d.USB).Include(d => d.FireWire).Include(d => d.PCMCIA).
                                        FirstOrDefaultAsync(m => m.Id == id)
            };

            if(model.Report is null)
            {
                return NotFound();
            }

            model.ReportAll = _context.
                              Reports.Where(d => d.Manufacturer == model.Report.Manufacturer &&
                                                 d.Model        == model.Report.Model        &&
                                                 d.Revision     == model.Report.Revision).
                              Select(d => d.Id).ToList();

            model.ReportButManufacturer = _context.
                                          Reports.Where(d => d.Model    == model.Report.Model &&
                                                             d.Revision == model.Report.Revision).Select(d => d.Id).
                                          Where(d => model.ReportAll.All(r => r != d)).ToList();

            model.SameAll = _context.
                            Devices.Where(d => d.Manufacturer == model.Report.Manufacturer &&
                                               d.Model        == model.Report.Model        &&
                                               d.Revision     == model.Report.Revision     &&
                                               d.Id           != id).Select(d => d.Id).ToList();

            model.SameButManufacturer = _context.
                                        Devices.Where(d => d.Model    == model.Report.Model    &&
                                                           d.Revision == model.Report.Revision && d.Id != id).
                                        Select(d => d.Id).Where(d => model.SameAll.All(r => r != d)).ToList();

            model.StatsAll = _context.DeviceStats.Include(d => d.Report).
                                      Where(d => d.Manufacturer == model.Report.Manufacturer &&
                                                 d.Model        == model.Report.Model        &&
                                                 d.Revision     == model.Report.Revision     &&
                                                 d.Report.Id    != model.Report.Id).ToList();

            model.StatsButManufacturer = _context.DeviceStats.Include(d => d.Report).
                                                  Where(d => d.Model     == model.Report.Model    &&
                                                             d.Revision  == model.Report.Revision &&
                                                             d.Report.Id != model.Report.Id).AsEnumerable().
                                                  Where(d => model.StatsAll.All(s => s.Id != d.Id)).ToList();

            model.ReadCapabilitiesId =
                model.Report.ATA?.ReadCapabilities?.Id ?? model.Report.SCSI?.ReadCapabilities?.Id ?? 0;

            // So we can check, as we know IDs with 0 will never exist, and EFCore does not allow null propagation in the LINQ
            int ataId   = model.Report.ATA?.Id                    ?? 0;
            int atapiId = model.Report.ATAPI?.Id                  ?? 0;
            int scsiId  = model.Report.SCSI?.Id                   ?? 0;
            int mmcId   = model.Report.SCSI?.MultiMediaDevice?.Id ?? 0;

            model.TestedMedias = _context.TestedMedia.
                                          Where(t => t.AtaId == ataId || t.AtaId == atapiId || t.ScsiId == scsiId ||
                                                     t.MmcId == mmcId).OrderBy(t => t.Manufacturer).
                                          ThenBy(t => t.Model).ThenBy(t => t.MediumTypeName).ToList();

            return View(model);
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
            int id, [Bind("OptimalMultipleSectorsRead,Id,CompactFlash,Manufacturer,Model,Revision,Type")]
            Device device)
        {
            if(id != device.Id)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
                return View(device);

            try
            {
                device.ModifiedWhen = DateTime.UtcNow;
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