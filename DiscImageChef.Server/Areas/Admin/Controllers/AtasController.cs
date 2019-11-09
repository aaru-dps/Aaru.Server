using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class AtasController : Controller
    {
        readonly DicServerContext _context;

        public AtasController(DicServerContext context) => _context = context;

        // GET: Admin/Atas
        public IActionResult Index() => View(_context.Ata.AsEnumerable().Where(m => m.IdentifyDevice?.Model != null).
                                                      OrderBy(m => m.IdentifyDevice.Value.Model));

        // GET: Admin/Atas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            CommonTypes.Metadata.Ata ata = await _context.Ata.FirstOrDefaultAsync(m => m.Id == id);

            if(ata == null)
            {
                return NotFound();
            }

            return View(ata);
        }

        // GET: Admin/Atas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            CommonTypes.Metadata.Ata ata = await _context.Ata.FirstOrDefaultAsync(m => m.Id == id);

            if(ata == null)
            {
                return NotFound();
            }

            return View(ata);
        }

        // POST: Admin/Atas/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            CommonTypes.Metadata.Ata ata = await _context.Ata.FindAsync(id);
            _context.Ata.Remove(ata);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool AtaExists(int id) => _context.Ata.Any(e => e.Id == id);

        public IActionResult Consolidate()
        {
            List<IdHashModel> hashes =
                _context.Ata.Select(m => new IdHashModel(m.Id, Hash.Sha512(m.Identify))).ToList();

            List<IdHashModel> dups = hashes.GroupBy(x => x.Hash).Where(g => g.Count() > 1).
                                            Select(x => hashes.FirstOrDefault(y => y.Hash == x.Key)).ToList();

            for(int i = 0; i < dups.Count; i++)
            {
                CommonTypes.Metadata.Ata unique = _context.Ata.First(a => a.Id == dups[i].Id);

                dups[i].Description = unique.IdentifyDevice?.Model;
                dups[i].Duplicates  = hashes.Where(h => h.Hash == dups[i].Hash).Skip(1).Select(x => x.Id).ToArray();
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
                foreach(int duplicateId in duplicate.Duplicates)
                {
                    foreach(Device ataDevice in _context.Devices.Where(d => d.ATAId == duplicateId))
                    {
                        ataDevice.ATAId = duplicate.Id;
                    }

                    foreach(Device atapiDevice in _context.Devices.Where(d => d.ATAPIId == duplicateId))
                    {
                        atapiDevice.ATAPIId = duplicate.Id;
                    }

                    foreach(UploadedReport ataReport in _context.Reports.Where(d => d.ATAId == duplicateId))
                    {
                        ataReport.ATAId = duplicate.Id;
                    }

                    foreach(UploadedReport atapiReport in _context.Reports.Where(d => d.ATAPIId == duplicateId))
                    {
                        atapiReport.ATAPIId = duplicate.Id;
                    }

                    _context.Ata.Remove(_context.Ata.First(d => d.Id == duplicateId));
                }
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}