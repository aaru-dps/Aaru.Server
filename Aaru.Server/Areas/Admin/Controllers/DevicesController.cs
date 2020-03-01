using System;
using System.Linq;
using System.Threading.Tasks;
using Aaru.CommonTypes.Metadata;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class DevicesController : Controller
    {
        readonly AaruServerContext _context;

        public DevicesController(AaruServerContext context) => _context = context;

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
                Report = await _context.Devices.FirstOrDefaultAsync(m => m.Id == id)
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

            model.StatsAll = _context.DeviceStats.
                                      Where(d => d.Manufacturer == model.Report.Manufacturer &&
                                                 d.Model        == model.Report.Model        &&
                                                 d.Revision     == model.Report.Revision     &&
                                                 d.Report.Id    != model.Report.Id).ToList();

            model.StatsButManufacturer = _context.DeviceStats.
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
            Device changedModel)
        {
            if(id != changedModel.Id)
                return NotFound();

            if(!ModelState.IsValid)
                return View(changedModel);

            Device model = await _context.Devices.FirstOrDefaultAsync(m => m.Id == id);

            if(model is null)
                return NotFound();

            model.OptimalMultipleSectorsRead = changedModel.OptimalMultipleSectorsRead;
            model.CompactFlash               = changedModel.CompactFlash;
            model.Manufacturer               = changedModel.Manufacturer;
            model.Model                      = changedModel.Model;
            model.Revision                   = changedModel.Revision;
            model.Type                       = changedModel.Type;
            model.ModifiedWhen               = DateTime.UtcNow;

            try
            {
                _context.Update(model);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("Concurrency", "Concurrency error, please report to the administrator.");
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

        public IActionResult Merge(int? master, int? slave)
        {
            if(master is null ||
               slave is null)
                return NotFound();

            Device masterDevice = _context.Devices.FirstOrDefault(m => m.Id == master);
            Device slaveDevice  = _context.Devices.FirstOrDefault(m => m.Id == slave);

            if(masterDevice is null ||
               slaveDevice is null)
                return NotFound();

            if(masterDevice.ATAId != null &&
               masterDevice.ATAId != slaveDevice.ATAId)
            {
                foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == slaveDevice.ATAId))
                {
                    testedMedia.AtaId = masterDevice.ATAId;
                    _context.Update(testedMedia);
                }
            }
            else if(masterDevice.ATAId == null &&
                    slaveDevice.ATAId  != null)
            {
                masterDevice.ATAId = slaveDevice.ATAId;
                _context.Update(masterDevice);
            }

            if(masterDevice.ATAPIId != null &&
               masterDevice.ATAPIId != slaveDevice.ATAPIId)
            {
                foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == slaveDevice.ATAPIId))
                {
                    testedMedia.AtaId = masterDevice.ATAPIId;
                    _context.Update(testedMedia);
                }
            }
            else if(masterDevice.ATAPIId == null &&
                    slaveDevice.ATAPIId  != null)
            {
                masterDevice.ATAPIId = slaveDevice.ATAPIId;
                _context.Update(masterDevice);
            }

            if(masterDevice.SCSIId != null &&
               masterDevice.SCSIId != slaveDevice.SCSIId)
            {
                foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.ScsiId == slaveDevice.SCSIId))
                {
                    testedMedia.ScsiId = masterDevice.SCSIId;
                    _context.Update(testedMedia);
                }
            }
            else if(masterDevice.SCSIId == null &&
                    slaveDevice.SCSIId  != null)
            {
                masterDevice.SCSIId = slaveDevice.SCSIId;
                _context.Update(masterDevice);
            }

            masterDevice.ModifiedWhen = DateTime.UtcNow;
            _context.Update(masterDevice);
            _context.Remove(slaveDevice);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new
            {
                Id = master
            });
        }

        public IActionResult MergeReports(int? deviceId, int? reportId)
        {
            if(deviceId is null ||
               reportId is null)
                return NotFound();

            Device         device = _context.Devices.FirstOrDefault(m => m.Id == deviceId);
            UploadedReport report = _context.Reports.FirstOrDefault(m => m.Id == reportId);

            if(device is null ||
               report is null)
                return NotFound();

            if(device.ATAId != null &&
               device.ATAId != report.ATAId)
            {
                foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == report.ATAId))
                {
                    testedMedia.AtaId = device.ATAId;
                    _context.Update(testedMedia);
                }

                if(device.ATA != null                  &&
                   device.ATA.ReadCapabilities is null &&
                   report.ATA?.ReadCapabilities != null)
                {
                    device.ATA.ReadCapabilities = report.ATA.ReadCapabilities;
                    _context.Update(device.ATA);
                }
            }
            else if(device.ATAId == null &&
                    report.ATAId != null)
            {
                device.ATAId = report.ATAId;
                _context.Update(device);
            }

            if(device.ATAPIId != null &&
               device.ATAPIId != report.ATAPIId)
            {
                foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == report.ATAPIId))
                {
                    testedMedia.AtaId = device.ATAPIId;
                    _context.Update(testedMedia);
                }
            }
            else if(device.ATAPIId == null &&
                    report.ATAPIId != null)
            {
                device.ATAPIId = report.ATAPIId;
                _context.Update(device);
            }

            if(device.SCSIId != null &&
               device.SCSIId != report.SCSIId)
            {
                foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.ScsiId == report.SCSIId))
                {
                    testedMedia.ScsiId = device.SCSIId;
                    _context.Update(testedMedia);
                }

                if(device.SCSI != null                  &&
                   device.SCSI.ReadCapabilities is null &&
                   report.SCSI?.ReadCapabilities != null)
                {
                    device.SCSI.ReadCapabilities = report.SCSI.ReadCapabilities;
                    _context.Update(device.SCSI);
                }

                if(device.SCSI != null                  &&
                   device.SCSI.MultiMediaDevice is null &&
                   report.SCSI?.MultiMediaDevice != null)
                {
                    device.SCSI.MultiMediaDevice = report.SCSI.MultiMediaDevice;
                    _context.Update(device.SCSI);
                }
                else if(device.SCSI?.MultiMediaDevice != null &&
                        report.SCSI?.MultiMediaDevice != null)
                {
                    foreach(TestedMedia testedMedia in
                        _context.TestedMedia.Where(d => d.MmcId == report.SCSI.MultiMediaDevice.Id))
                    {
                        testedMedia.MmcId = device.SCSI.MultiMediaDevice.Id;
                        _context.Update(testedMedia);
                    }
                }

                if(device.SCSI != null                  &&
                   device.SCSI.SequentialDevice is null &&
                   report.SCSI?.SequentialDevice != null)
                {
                    device.SCSI.SequentialDevice = report.SCSI.SequentialDevice;
                    _context.Update(device.SCSI);
                }
                else if(device.SCSI?.SequentialDevice != null &&
                        report.SCSI?.SequentialDevice != null)
                {
                    foreach(TestedSequentialMedia testedSequentialMedia in
                        _context.TestedSequentialMedia.Where(d => d.SscId == report.SCSI.SequentialDevice.Id))
                    {
                        testedSequentialMedia.SscId = device.SCSI.SequentialDevice.Id;
                        _context.Update(testedSequentialMedia);
                    }
                }
            }
            else if(device.SCSIId == null &&
                    report.SCSIId != null)
            {
                device.SCSIId = report.SCSIId;
                _context.Update(device);
            }

            _context.Remove(report);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new
            {
                Id = deviceId
            });
        }

        public IActionResult LinkReports(int? statsId, int? deviceId)
        {
            if(statsId is null ||
               deviceId is null)
                return NotFound();

            Device     device = _context.Devices.FirstOrDefault(m => m.Id     == deviceId);
            DeviceStat stat   = _context.DeviceStats.FirstOrDefault(m => m.Id == statsId);

            if(device is null ||
               stat is null)
                return NotFound();

            stat.Report = device;
            _context.Update(stat);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new
            {
                Id = deviceId
            });
        }

        public IActionResult Find(int id, string manufacturer, string model, string revision, string bus)
        {
            if(model is null)
                return NotFound();

            var found = new FindReportModel
            {
                Id          = id, Manufacturer = manufacturer, Model = model, Revision = revision,
                Bus         = bus,
                LikeDevices = _context.Devices.Where(r => r.Model.ToLower().Contains(model.ToLower())).ToList()
            };

            return View(found);
        }
    }
}