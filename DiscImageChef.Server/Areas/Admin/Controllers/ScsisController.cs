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
    public class ScsisController : Controller
    {
        readonly DicServerContext _context;

        public ScsisController(DicServerContext context) => _context = context;

        // GET: Admin/Scsis
        public async Task<IActionResult> Index() => View(await _context.Scsi.ToListAsync());

        // GET: Admin/Scsis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Scsi scsi = await _context.Scsi.FirstOrDefaultAsync(m => m.Id == id);

            if(scsi == null)
            {
                return NotFound();
            }

            return View(scsi);
        }

        // GET: Admin/Scsis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Scsi scsi = await _context.Scsi.FirstOrDefaultAsync(m => m.Id == id);

            if(scsi == null)
            {
                return NotFound();
            }

            return View(scsi);
        }

        // POST: Admin/Scsis/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Scsi scsi = await _context.Scsi.FindAsync(id);
            _context.Scsi.Remove(scsi);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Consolidate()
        {
            List<IdHashModel> hashes = _context.Scsi.Where(m => m.InquiryData != null).
                                                Select(m => new IdHashModel(m.Id, Hash.Sha512(m.InquiryData))).ToList();

            List<IdHashModel> dups = hashes.GroupBy(x => x.Hash).Where(g => g.Count() > 1).
                                            Select(x => hashes.FirstOrDefault(y => y.Hash == x.Key)).ToList();

            for(int i = 0; i < dups.Count; i++)
            {
                Scsi unique = _context.Scsi.First(a => a.Id == dups[i].Id);

                dups[i].Description =
                    $"{StringHandlers.CToString(unique.Inquiry?.VendorIdentification)} {StringHandlers.CToString(unique.Inquiry?.ProductIdentification)}";

                dups[i].Duplicates = hashes.Where(h => h.Hash == dups[i].Hash).Skip(1).Select(x => x.Id).ToArray();
            }

            return View(new IdHashModelForView
            {
                List = dups, Json = JsonConvert.SerializeObject(dups)
            });
        }

        [HttpPost, ActionName("Consolidate"), ValidateAntiForgeryToken]
        public IActionResult ConsolidateConfirmed(string models)
        {
            IdHashModel[] duplicates;

            try
            {
                duplicates = JsonConvert.DeserializeObject<IdHashModel[]>(models);
            }
            catch(JsonSerializationException)
            {
                return BadRequest();
            }

            if(duplicates is null)
                return BadRequest();

            foreach(IdHashModel duplicate in duplicates)
            {
                Scsi master = _context.Scsi.FirstOrDefault(m => m.Id == duplicate.Id);

                if(master is null)
                    continue;

                foreach(int duplicateId in duplicate.Duplicates)
                {
                    Scsi slave = _context.Scsi.FirstOrDefault(m => m.Id == duplicateId);

                    if(slave is null)
                        continue;

                    foreach(Device scsiDevice in _context.Devices.Where(d => d.SCSI.Id == duplicateId))
                    {
                        scsiDevice.SCSI = master;
                    }

                    foreach(UploadedReport scsiReport in _context.Reports.Where(d => d.SCSI.Id == duplicateId))
                    {
                        scsiReport.SCSI = master;
                    }

                    foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.ScsiId == duplicateId))
                    {
                        testedMedia.ScsiId = duplicate.Id;
                        _context.Update(testedMedia);
                    }

                    if(master.ReadCapabilities is null &&
                       slave.ReadCapabilities != null)
                        master.ReadCapabilities = slave.ReadCapabilities;

                    _context.Scsi.Remove(slave);
                }
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}