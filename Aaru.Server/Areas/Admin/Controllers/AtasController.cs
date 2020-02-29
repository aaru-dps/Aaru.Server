using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Aaru.Server.Models;
using Aaru.CommonTypes.Metadata;
using Aaru.CommonTypes.Structs.Devices.ATA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Aaru.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class AtasController : Controller
    {
        readonly DicServerContext _context;

        public AtasController(DicServerContext context) => _context = context;

        // GET: Admin/Atas
        public IActionResult Index() => View(_context.Ata.AsEnumerable().OrderBy(m => m.IdentifyDevice?.Model).
                                                      ThenBy(m => m.IdentifyDevice?.FirmwareRevision));

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
                CommonTypes.Metadata.Ata master = _context.Ata.FirstOrDefault(m => m.Id == duplicate.Id);

                if(master is null)
                    continue;

                foreach(int duplicateId in duplicate.Duplicates)
                {
                    CommonTypes.Metadata.Ata slave = _context.Ata.FirstOrDefault(m => m.Id == duplicateId);

                    if(slave is null)
                        continue;

                    foreach(Device ataDevice in _context.Devices.Where(d => d.ATA.Id == duplicateId))
                    {
                        ataDevice.ATA = master;
                    }

                    foreach(Device atapiDevice in _context.Devices.Where(d => d.ATAPI.Id == duplicateId))
                    {
                        atapiDevice.ATAPI = master;
                    }

                    foreach(UploadedReport ataReport in _context.Reports.Where(d => d.ATA.Id == duplicateId))
                    {
                        ataReport.ATA = master;
                    }

                    foreach(UploadedReport atapiReport in _context.Reports.Where(d => d.ATAPI.Id == duplicateId))
                    {
                        atapiReport.ATAPI = master;
                    }

                    foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == duplicateId))
                    {
                        testedMedia.AtaId = duplicate.Id;
                        _context.Update(testedMedia);
                    }

                    if(master.ReadCapabilities is null &&
                       slave.ReadCapabilities != null)
                        master.ReadCapabilities = slave.ReadCapabilities;

                    _context.Ata.Remove(slave);
                }
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ConsolidateWithIds(int masterId, int slaveId)
        {
            CommonTypes.Metadata.Ata master = _context.Ata.FirstOrDefault(m => m.Id == masterId);

            if(master is null)
                return RedirectToAction(nameof(Compare), new
                {
                    id = masterId, rightId = slaveId
                });

            CommonTypes.Metadata.Ata slave = _context.Ata.FirstOrDefault(m => m.Id == slaveId);

            if(slave is null)
                return RedirectToAction(nameof(Compare), new
                {
                    id = masterId, rightId = slaveId
                });

            foreach(Device ataDevice in _context.Devices.Where(d => d.ATA.Id == slaveId))
            {
                ataDevice.ATA = master;
            }

            foreach(Device atapiDevice in _context.Devices.Where(d => d.ATAPI.Id == slaveId))
            {
                atapiDevice.ATAPI = master;
            }

            foreach(UploadedReport ataReport in _context.Reports.Where(d => d.ATA.Id == slaveId))
            {
                ataReport.ATA = master;
            }

            foreach(UploadedReport atapiReport in _context.Reports.Where(d => d.ATAPI.Id == slaveId))
            {
                atapiReport.ATAPI = master;
            }

            foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.AtaId == slaveId))
            {
                testedMedia.AtaId = masterId;
                _context.Update(testedMedia);
            }

            if(master.ReadCapabilities is null &&
               slave.ReadCapabilities != null)
                master.ReadCapabilities = slave.ReadCapabilities;

            _context.Ata.Remove(slave);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Compare(int id, int rightId)
        {
            var model = new CompareModel
            {
                LeftId = id, RightId = rightId
            };

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
            model.ValueNames  = new List<string>();
            model.LeftValues  = new List<string>();
            model.RightValues = new List<string>();

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

            foreach(FieldInfo fieldInfo in leftValue.GetType().GetFields())
            {
                object lv = fieldInfo.GetValue(leftValue);
                object rv = fieldInfo.GetValue(rightValue);

                if(fieldInfo.FieldType.IsArray)
                {
                    var la = lv as Array;
                    var ra = rv as Array;

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

                    List<object> ll = la.Cast<object>().ToList();
                    List<object> rl = ra.Cast<object>().ToList();

                    for(int i = 0; i < ll.Count; i++)
                    {
                        if(ll[i].Equals(rl[i]))
                            continue;

                        model.ValueNames.Add(fieldInfo.Name);
                        model.LeftValues.Add("[]");
                        model.RightValues.Add("[]");

                        break;
                    }
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

        public IActionResult CheckPrivate()
        {
            List<CommonTypes.Metadata.Ata> havePrivacy = new List<CommonTypes.Metadata.Ata>();
            byte[]                         tmp;

            foreach(CommonTypes.Metadata.Ata ata in _context.Ata)
            {
                Identify.IdentifyDevice? id = ata.IdentifyDevice;

                if(id is null)
                    continue;

                if(!string.IsNullOrWhiteSpace(id.Value.SerialNumber) ||
                   id.Value.WWN          != 0                        ||
                   id.Value.WWNExtension != 0                        ||
                   !string.IsNullOrWhiteSpace(id.Value.MediaSerial))
                {
                    havePrivacy.Add(ata);

                    continue;
                }

                tmp = new byte[10];
                Array.Copy(ata.Identify, 121 * 2, tmp, 0, 10);

                if(tmp.All(b => b > 0x20) &&
                   tmp.All(b => b <= 0x5F))
                {
                    havePrivacy.Add(ata);

                    continue;
                }

                tmp = new byte[62];
                Array.Copy(ata.Identify, 129 * 2, tmp, 0, 62);

                if(tmp.All(b => b > 0x20) &&
                   tmp.All(b => b <= 0x5F))
                {
                    havePrivacy.Add(ata);

                    continue;
                }

                tmp = new byte[14];
                Array.Copy(ata.Identify, 161 * 2, tmp, 0, 14);

                if(tmp.All(b => b > 0x20) &&
                   tmp.All(b => b <= 0x5F))
                {
                    havePrivacy.Add(ata);

                    continue;
                }

                tmp = new byte[12];
                Array.Copy(ata.Identify, 224 * 2, tmp, 0, 12);

                if(tmp.All(b => b > 0x20) &&
                   tmp.All(b => b <= 0x5F))
                {
                    havePrivacy.Add(ata);

                    continue;
                }

                tmp = new byte[38];
                Array.Copy(ata.Identify, 236 * 2, tmp, 0, 38);

                if(tmp.All(b => b > 0x20) &&
                   tmp.All(b => b <= 0x5F))
                {
                    havePrivacy.Add(ata);
                }
            }

            return View(havePrivacy);
        }

        public IActionResult ClearPrivate(int id)
        {
            CommonTypes.Metadata.Ata ata = _context.Ata.FirstOrDefault(a => a.Id == id);

            if(ata is null)
                return RedirectToAction(nameof(CheckPrivate));

            // Serial number
            for(int i = 0; i < 20; i++)
                ata.Identify[(10 * 2) + i] = 0x20;

            // Media serial number
            for(int i = 0; i < 40; i++)
                ata.Identify[(176 * 2) + i] = 0x20;

            // WWN and WWN Extension
            for(int i = 0; i < 16; i++)
                ata.Identify[(108 * 2) + i] = 0;

            // We need to tell EFCore the entity has changed
            _context.Update(ata);
            _context.SaveChanges();

            return RedirectToAction(nameof(CheckPrivate));
        }

        public IActionResult ClearReserved(int id)
        {
            CommonTypes.Metadata.Ata ata = _context.Ata.FirstOrDefault(a => a.Id == id);

            if(ata is null)
                return RedirectToAction(nameof(CheckPrivate));

            // ReservedWords121
            for(int i = 0; i < 10; i++)
                ata.Identify[(121 * 2) + i] = 0;

            // ReservedWords129
            for(int i = 0; i < 40; i++)
                ata.Identify[(129 * 2) + i] = 0;

            // ReservedCFA
            for(int i = 0; i < 14; i++)
                ata.Identify[(161 * 2) + i] = 0;

            // ReservedCEATA224
            for(int i = 0; i < 12; i++)
                ata.Identify[(224 * 2) + i] = 0;

            // ReservedWords
            for(int i = 0; i < 14; i++)
                ata.Identify[(161 * 2) + i] = 0;

            // We need to tell EFCore the entity has changed
            _context.Update(ata);
            _context.SaveChanges();

            return RedirectToAction(nameof(CheckPrivate));
        }

        public IActionResult ClearPrivateAll()
        {
            foreach(CommonTypes.Metadata.Ata ata in _context.Ata)
            {
                if(ata is null)
                    return RedirectToAction(nameof(CheckPrivate));

                // Serial number
                for(int i = 0; i < 20; i++)
                    ata.Identify[(10 * 2) + i] = 0x20;

                // Media serial number
                for(int i = 0; i < 40; i++)
                    ata.Identify[(176 * 2) + i] = 0x20;

                // WWN and WWN Extension
                for(int i = 0; i < 16; i++)
                    ata.Identify[(108 * 2) + i] = 0;

                // We need to tell EFCore the entity has changed
                _context.Update(ata);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(CheckPrivate));
        }

        public IActionResult ClearReservedAll()
        {
            foreach(CommonTypes.Metadata.Ata ata in _context.Ata)
            {
                // ReservedWords121
                for(int i = 0; i < 10; i++)
                    ata.Identify[(121 * 2) + i] = 0;

                // ReservedWords129
                for(int i = 0; i < 40; i++)
                    ata.Identify[(129 * 2) + i] = 0;

                // ReservedCFA
                for(int i = 0; i < 14; i++)
                    ata.Identify[(161 * 2) + i] = 0;

                // ReservedCEATA224
                for(int i = 0; i < 12; i++)
                    ata.Identify[(224 * 2) + i] = 0;

                // ReservedWords
                for(int i = 0; i < 14; i++)
                    ata.Identify[(161 * 2) + i] = 0;

                // We need to tell EFCore the entity has changed
                _context.Update(ata);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(CheckPrivate));
        }
    }
}