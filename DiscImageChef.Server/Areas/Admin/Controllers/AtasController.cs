using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using DiscImageChef.Decoders.ATA;
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

                    foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == duplicateId))
                    {
                        testedMedia.AtaId = duplicate.Id;
                    }

                    _context.Ata.Remove(_context.Ata.First(d => d.Id == duplicateId));
                }
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ConsolidateWithIds(int masterId, int slaveId)
        {
            if(_context.Ata.Count(m => m.Id == masterId) == 0)
                return RedirectToAction(nameof(Compare), new
                {
                    id = masterId, rightId = slaveId
                });

            if(_context.Ata.Count(m => m.Id == slaveId) == 0)
                return RedirectToAction(nameof(Compare), new
                {
                    id = masterId, rightId = slaveId
                });

            foreach(Device ataDevice in _context.Devices.Where(d => d.ATAId == slaveId))
            {
                ataDevice.ATAId = masterId;
            }

            foreach(Device atapiDevice in _context.Devices.Where(d => d.ATAPIId == slaveId))
            {
                atapiDevice.ATAPIId = masterId;
            }

            foreach(UploadedReport ataReport in _context.Reports.Where(d => d.ATAId == slaveId))
            {
                ataReport.ATAId = masterId;
            }

            foreach(UploadedReport atapiReport in _context.Reports.Where(d => d.ATAPIId == slaveId))
            {
                atapiReport.ATAPIId = masterId;
            }

            foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == slaveId))
            {
                testedMedia.AtaId = masterId;
            }

            _context.Ata.Remove(_context.Ata.First(d => d.Id == slaveId));

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Compare(int id, int rightId)
        {
            var model = new CompareModel();

            model.LeftId  = id;
            model.RightId = rightId;

            CommonTypes.Metadata.Ata left  = _context.Ata.FirstOrDefault(l => l.Id == id);
            CommonTypes.Metadata.Ata right = _context.Ata.FirstOrDefault(r => r.Id == rightId);

            if(left is null)
            {
                model.ErrorMessage = $"ATA with id {id} has not been found";
                model.HasError     = true;

                return View(model);
            }

            if(right is null)
            {
                model.ErrorMessage = $"ATA with id {rightId} has not been found";
                model.HasError     = true;

                return View(model);
            }

            Identify.IdentifyDevice? leftNullable  = left.IdentifyDevice;
            Identify.IdentifyDevice? rightNullable = right.IdentifyDevice;

            if(!leftNullable.HasValue &&
               !rightNullable.HasValue)
            {
                model.AreEqual = true;

                return View(model);
            }

            if(leftNullable.HasValue &&
               !rightNullable.HasValue)
            {
                model.ValueNames.Add("Decoded");
                model.LeftValues.Add("decoded");
                model.RightValues.Add("null");

                return View(model);
            }

            if(!leftNullable.HasValue)
            {
                model.ValueNames.Add("Decoded");
                model.LeftValues.Add("null");
                model.RightValues.Add("decoded");

                return View(model);
            }

            Identify.IdentifyDevice leftValue  = left.IdentifyDevice.Value;
            Identify.IdentifyDevice rightValue = right.IdentifyDevice.Value;
            model.ValueNames  = new List<string>();
            model.LeftValues  = new List<string>();
            model.RightValues = new List<string>();

            foreach(FieldInfo fieldInfo in leftValue.GetType().GetFields())
            {
                object lv = fieldInfo.GetValue(leftValue);
                object rv = fieldInfo.GetValue(rightValue);

                if(fieldInfo.FieldType.IsArray)
                {
                    object[] la = lv as object[];
                    object[] ra = rv as object[];

                    switch(la)
                    {
                        case null when ra is null: continue;
                        case null:
                            model.ValueNames.Add(fieldInfo.Name);
                            model.LeftValues.Add("null");
                            model.RightValues.Add("[]");

                            continue;
                    }

                    if(ra is null)
                    {
                        model.ValueNames.Add(fieldInfo.Name);
                        model.LeftValues.Add("[]");
                        model.RightValues.Add("null");

                        continue;
                    }

                    if(la.SequenceEqual(ra))
                        continue;

                    model.ValueNames.Add(fieldInfo.Name);
                    model.LeftValues.Add("[]");
                    model.RightValues.Add("[]");
                }
                else if(lv == null &&
                        rv == null) { }
                else if(lv != null &&
                        rv == null)
                {
                    model.ValueNames.Add(fieldInfo.Name);
                    model.LeftValues.Add($"{lv}");
                    model.RightValues.Add("null");
                }
                else if(lv == null)
                {
                    model.ValueNames.Add(fieldInfo.Name);
                    model.LeftValues.Add("null");
                    model.RightValues.Add($"{rv}");
                }
                else if(!lv.Equals(rv))

                {
                    model.ValueNames.Add(fieldInfo.Name);
                    model.LeftValues.Add($"{lv}");
                    model.RightValues.Add($"{rv}");
                }
            }

            model.AreEqual = model.LeftValues.Count == 0 && model.RightValues.Count == 0;

            return View(model);
        }
    }
}