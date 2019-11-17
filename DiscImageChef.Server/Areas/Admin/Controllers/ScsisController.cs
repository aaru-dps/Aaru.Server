using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using DiscImageChef.Decoders.SCSI;
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
        public IActionResult Index() => View(_context.Scsi.AsEnumerable().
                                                      OrderBy(m =>
                                                                  StringHandlers.CToString(m.Inquiry?.
                                                                                             VendorIdentification)).
                                                      ThenBy(m =>
                                                                 StringHandlers.CToString(m.Inquiry?.
                                                                                            ProductIdentification)).
                                                      ThenBy(m =>
                                                                 StringHandlers.CToString(m.Inquiry?.
                                                                                            ProductRevisionLevel)));

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

        public IActionResult Compare(int id, int rightId)
        {
            var model = new CompareModel
            {
                LeftId = id, RightId = rightId
            };

            Scsi left  = _context.Scsi.FirstOrDefault(l => l.Id == id);
            Scsi right = _context.Scsi.FirstOrDefault(r => r.Id == rightId);

            if(left is null)
            {
                model.ErrorMessage = $"SCSI with id {id} has not been found";
                model.HasError     = true;

                return View(model);
            }

            if(right is null)
            {
                model.ErrorMessage = $"SCSI with id {rightId} has not been found";
                model.HasError     = true;

                return View(model);
            }

            Inquiry.SCSIInquiry? leftNullable  = left.Inquiry;
            Inquiry.SCSIInquiry? rightNullable = right.Inquiry;
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

            Inquiry.SCSIInquiry leftValue  = left.Inquiry.Value;
            Inquiry.SCSIInquiry rightValue = right.Inquiry.Value;

            foreach(FieldInfo fieldInfo in leftValue.GetType().GetFields())
            {
                object lv = fieldInfo.GetValue(leftValue);
                object rv = fieldInfo.GetValue(rightValue);

                if(fieldInfo.FieldType.IsArray)
                {
                    Array la = lv as Array;
                    Array ra = rv as Array;

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

        public IActionResult ConsolidateWithIds(int masterId, int slaveId)
        {
            Scsi master = _context.Scsi.FirstOrDefault(m => m.Id == masterId);

            if(master is null)
                return RedirectToAction(nameof(Compare), new
                {
                    id = masterId, rightId = slaveId
                });

            Scsi slave = _context.Scsi.FirstOrDefault(m => m.Id == slaveId);

            if(slave is null)
                return RedirectToAction(nameof(Compare), new
                {
                    id = masterId, rightId = slaveId
                });

            foreach(Device scsiDevice in _context.Devices.Where(d => d.SCSI.Id == slaveId))
            {
                scsiDevice.SCSI = master;
            }

            foreach(UploadedReport scsiReport in _context.Reports.Where(d => d.SCSI.Id == slaveId))
            {
                scsiReport.SCSI = master;
            }

            foreach(TestedMedia testedMedia in _context.TestedMedia.Where(d => d.ScsiId == slaveId))
            {
                testedMedia.ScsiId = masterId;
                _context.Update(testedMedia);
            }

            if(master.ReadCapabilities is null &&
               slave.ReadCapabilities != null)
                master.ReadCapabilities = slave.ReadCapabilities;

            _context.Scsi.Remove(slave);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}