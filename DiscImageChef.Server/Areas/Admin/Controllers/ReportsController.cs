using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class ReportsController : Controller
    {
        readonly DicServerContext _context;

        public ReportsController(DicServerContext context) => _context = context;

        // GET: Admin/Reports
        public async Task<IActionResult> Index() =>
            View(await _context.Reports.OrderBy(r => r.Manufacturer).ThenBy(r => r.Model).ThenBy(r => r.Revision).
                                ThenBy(r => r.CompactFlash).ThenBy(r => r.Type).ToListAsync());

        // GET: Admin/Reports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var model = new UploadedReportDetails
            {
                Report = await _context.Reports.FirstOrDefaultAsync(m => m.Id == id)
            };

            if(model.Report is null)
            {
                return NotFound();
            }

            model.ReportAll = _context.
                              Devices.Where(d => d.Manufacturer == model.Report.Manufacturer &&
                                                 d.Model        == model.Report.Model        &&
                                                 d.Revision     == model.Report.Revision).
                              Select(d => d.Id).ToList();

            model.ReportButManufacturer = _context.
                                          Devices.Where(d => d.Model    == model.Report.Model &&
                                                             d.Revision == model.Report.Revision).Select(d => d.Id).
                                          Where(d => model.ReportAll.All(r => r != d)).ToList();

            model.SameAll = _context.
                            Reports.Where(d => d.Manufacturer == model.Report.Manufacturer &&
                                               d.Model        == model.Report.Model        &&
                                               d.Revision     == model.Report.Revision     &&
                                               d.Id           != id).Select(d => d.Id).ToList();

            model.SameButManufacturer = _context.
                                        Reports.Where(d => d.Model    == model.Report.Model    &&
                                                           d.Revision == model.Report.Revision && d.Id != id).
                                        Select(d => d.Id).Where(d => model.SameAll.All(r => r != d)).ToList();

            model.ReadCapabilitiesId =
                model.Report.ATA?.ReadCapabilities?.Id ?? model.Report.SCSI?.ReadCapabilities?.Id ?? 0;

            // So we can check, as we know IDs with 0 will never exist, and EFCore does not allow null propagation in the LINQ
            int ataId   = model.Report.ATA?.Id                    ?? 0;
            int atapiId = model.Report.ATAPI?.Id                  ?? 0;
            int scsiId  = model.Report.SCSI?.Id                   ?? 0;
            int mmcId   = model.Report.SCSI?.MultiMediaDevice?.Id ?? 0;
            int sscId   = model.Report.SCSI?.SequentialDevice?.Id ?? 0;

            model.TestedMedias = _context.TestedMedia.
                                          Where(t => t.AtaId == ataId || t.AtaId == atapiId || t.ScsiId == scsiId ||
                                                     t.MmcId == mmcId).OrderBy(t => t.Manufacturer).
                                          ThenBy(t => t.Model).ThenBy(t => t.MediumTypeName).ToList();

            model.TestedSequentialMedias = _context.TestedSequentialMedia.Where(t => t.SscId == sscId).
                                                    OrderBy(t => t.Manufacturer).ThenBy(t => t.Model).
                                                    ThenBy(t => t.MediumTypeName).ToList();

            return View(model);
        }

        // GET: Admin/Reports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UploadedReport uploadedReport = await _context.Reports.FindAsync(id);

            if(uploadedReport == null)
            {
                return NotFound();
            }

            return View(uploadedReport);
        }

        // POST: Admin/Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CompactFlash,Manufacturer,Model,Revision,Type")]
                                              UploadedReport uploadedReport)
        {
            if(id != uploadedReport.Id)
            {
                return NotFound();
            }

            if(!ModelState.IsValid)
                return View(uploadedReport);

            try
            {
                _context.Update(uploadedReport);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!UploadedReportExists(uploadedReport.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Reports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UploadedReport uploadedReport = await _context.Reports.FirstOrDefaultAsync(m => m.Id == id);

            if(uploadedReport == null)
            {
                return NotFound();
            }

            return View(uploadedReport);
        }

        // POST: Admin/Reports/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            UploadedReport uploadedReport = await _context.Reports.FindAsync(id);
            _context.Reports.Remove(uploadedReport);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool UploadedReportExists(int id) => _context.Reports.Any(e => e.Id == id);

        public IActionResult Promote(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            UploadedReport uploadedReport = _context.Reports.FirstOrDefault(m => m.Id == id);

            if(uploadedReport == null)
            {
                return NotFound();
            }

            var device = new Device(uploadedReport.ATAId, uploadedReport.ATAPIId, uploadedReport.FireWireId,
                                    uploadedReport.MultiMediaCardId, uploadedReport.PCMCIAId,
                                    uploadedReport.SecureDigitalId, uploadedReport.SCSIId, uploadedReport.USBId,
                                    uploadedReport.UploadedWhen, uploadedReport.Manufacturer, uploadedReport.Model,
                                    uploadedReport.Revision, uploadedReport.CompactFlash, uploadedReport.Type);

            EntityEntry<Device> res = _context.Devices.Add(device);
            _context.Reports.Remove(uploadedReport);
            _context.SaveChanges();

            return RedirectToAction(nameof(DevicesController.Details), "Devices", new
            {
                id = res.Entity.Id
            });
        }

        public IActionResult Merge(int? master, int? slave)
        {
            if(master is null ||
               slave is null)
                return NotFound();

            UploadedReport masterReport = _context.Reports.FirstOrDefault(m => m.Id == master);
            UploadedReport slaveReport  = _context.Reports.FirstOrDefault(m => m.Id == slave);

            if(masterReport is null ||
               slaveReport is null)
                return NotFound();

            if(masterReport.ATAId != null &&
               masterReport.ATAId != slaveReport.ATAId)
            {
                foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == slaveReport.ATAId))
                {
                    testedMedia.AtaId = masterReport.ATAId;
                    _context.Update(testedMedia);
                }
            }
            else if(masterReport.ATAId == null &&
                    slaveReport.ATAId  != null)
            {
                masterReport.ATAId = slaveReport.ATAId;
                _context.Update(masterReport);
            }

            if(masterReport.ATAPIId != null &&
               masterReport.ATAPIId != slaveReport.ATAPIId)
            {
                foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == slaveReport.ATAPIId))
                {
                    testedMedia.AtaId = masterReport.ATAPIId;
                    _context.Update(testedMedia);
                }
            }
            else if(masterReport.ATAPIId == null &&
                    slaveReport.ATAPIId  != null)
            {
                masterReport.ATAPIId = slaveReport.ATAPIId;
                _context.Update(masterReport);
            }

            if(masterReport.SCSIId != null &&
               masterReport.SCSIId != slaveReport.SCSIId)
            {
                foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.ScsiId == slaveReport.SCSIId))
                {
                    testedMedia.ScsiId = masterReport.SCSIId;
                    _context.Update(testedMedia);
                }
            }
            else if(masterReport.SCSIId == null &&
                    slaveReport.SCSIId  != null)
            {
                masterReport.SCSIId = slaveReport.SCSIId;
                _context.Update(masterReport);
            }

            if(masterReport.SCSI?.SequentialDeviceId != null &&
               masterReport.SCSI?.SequentialDeviceId != slaveReport.SCSI?.SequentialDeviceId)
            {
                foreach(TestedSequentialMedia testedMedia in
                    _context.TestedSequentialMedia.Where(d => d.SscId == slaveReport.SCSI.SequentialDeviceId))
                {
                    testedMedia.SscId = masterReport.SCSI.SequentialDeviceId;
                    _context.Update(testedMedia);
                }
            }
            else if(masterReport.SCSI                     != null &&
                    masterReport.SCSI?.SequentialDeviceId == null &&
                    slaveReport.SCSI?.SequentialDeviceId  != null)
            {
                masterReport.SCSI.SequentialDeviceId = slaveReport.SCSI.SequentialDeviceId;
                _context.Update(masterReport);
            }

            _context.Remove(slaveReport);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new
            {
                Id = master
            });
        }
    }
}