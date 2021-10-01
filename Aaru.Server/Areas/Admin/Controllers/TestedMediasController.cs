using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Aaru.CommonTypes.Metadata;
using Aaru.CommonTypes.Structs.Devices.SCSI;
using Aaru.Decoders.ATA;
using Aaru.Decoders.Bluray;
using Aaru.Decoders.CD;
using Aaru.Decoders.DVD;
using Aaru.Decoders.SCSI;
using Aaru.Helpers;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Cartridge = Aaru.Decoders.Bluray.Cartridge;
using DDS = Aaru.Decoders.Bluray.DDS;
using DMI = Aaru.Decoders.Xbox.DMI;
using MediaType = Aaru.CommonTypes.MediaType;
using Spare = Aaru.Decoders.Bluray.Spare;

namespace Aaru.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public sealed class TestedMediasController : Controller
    {
        readonly AaruServerContext _context;

        public TestedMediasController(AaruServerContext context) => _context = context;

        // GET: Admin/TestedMedias
        public async Task<IActionResult> Index() => View(await _context.TestedMedia.OrderBy(m => m.Manufacturer).
                                                                        ThenBy(m => m.Model).
                                                                        ThenBy(m => m.MediumTypeName).
                                                                        ThenBy(m => m.MediaIsRecognized).
                                                                        ThenBy(m => m.LongBlockSize).
                                                                        ThenBy(m => m.BlockSize).ThenBy(m => m.Blocks).
                                                                        ToListAsync());

        // GET: Admin/TestedMedias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            TestedMedia testedMedia = await _context.TestedMedia.FirstOrDefaultAsync(m => m.Id == id);

            if(testedMedia == null)
            {
                return NotFound();
            }

            return View(testedMedia);
        }

        // GET: Admin/TestedMedias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            TestedMedia testedMedia = await _context.TestedMedia.FindAsync(id);

            if(testedMedia == null)
            {
                return NotFound();
            }

            return View(testedMedia);
        }

        // POST: Admin/TestedMedias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("Id,Blocks,BlockSize,LongBlockSize,Manufacturer,MediumTypeName,Model")]
            TestedMedia changedModel)
        {
            if(id != changedModel.Id)
                return NotFound();

            if(!ModelState.IsValid)
                return View(changedModel);

            TestedMedia model = await _context.TestedMedia.FirstOrDefaultAsync(m => m.Id == id);

            if(model is null)
                return NotFound();

            model.Blocks         = changedModel.Blocks;
            model.BlockSize      = changedModel.BlockSize;
            model.LongBlockSize  = changedModel.LongBlockSize;
            model.Manufacturer   = changedModel.Manufacturer;
            model.MediumTypeName = changedModel.MediumTypeName;
            model.Model          = changedModel.Model;

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

        // GET: Admin/TestedMedias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            TestedMedia testedMedia = await _context.TestedMedia.FirstOrDefaultAsync(m => m.Id == id);

            if(testedMedia == null)
            {
                return NotFound();
            }

            return View(testedMedia);
        }

        // POST: Admin/TestedMedias/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TestedMedia testedMedia = await _context.TestedMedia.FindAsync(id);
            _context.TestedMedia.Remove(testedMedia);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ViewData(int id, string data)
        {
            if(string.IsNullOrWhiteSpace(data))
                return NotFound();

            TestedMedia testedMedia = _context.TestedMedia.FirstOrDefault(m => m.Id == id);

            if(testedMedia == null)
            {
                return NotFound();
            }

            var model = new TestedMediaDataModel
            {
                TestedMediaId = id,
                DataName      = data
            };

            byte[] buffer;
            byte[] sector    = new byte[2352];
            byte[] subq      = new byte[16];
            byte[] fullsub   = new byte[96];
            bool   c2Errors  = false;
            bool   scrambled = false;

            switch(data)
            {
                case nameof(testedMedia.AdipData):
                    buffer = testedMedia.AdipData;

                    break;
                case nameof(testedMedia.AtipData):
                    buffer        = testedMedia.AtipData;
                    model.Decoded = ATIP.Prettify(buffer);

                    break;
                case nameof(testedMedia.BluBcaData):
                    buffer = testedMedia.BluBcaData;

                    break;
                case nameof(testedMedia.BluDdsData):
                    buffer        = testedMedia.BluDdsData;
                    model.Decoded = DDS.Prettify(buffer);

                    break;
                case nameof(testedMedia.BluDiData):
                    buffer        = testedMedia.BluDiData;
                    model.Decoded = DI.Prettify(buffer);

                    break;
                case nameof(testedMedia.BluPacData):
                    buffer        = testedMedia.BluPacData;
                    model.Decoded = Cartridge.Prettify(buffer);

                    break;
                case nameof(testedMedia.BluSaiData):
                    buffer        = testedMedia.BluSaiData;
                    model.Decoded = Spare.Prettify(buffer);

                    break;
                case nameof(testedMedia.C2PointersData):
                    buffer = testedMedia.C2PointersData;

                    if(buffer is null       ||
                       buffer.Length < 2352 ||
                       buffer.All(c => c == 0))
                        break;

                    Array.Copy(buffer, 0, sector, 0, 2352);

                    model.Decoded = Sector.Prettify(sector);

                    for(int i = 2352; i < buffer.Length; i++)
                    {
                        if(buffer[i] == 0x00)
                            continue;

                        c2Errors = true;

                        break;
                    }

                    model.Decoded += "\n" + (c2Errors ? "C2 errors found." : "No C2 errors.");

                    break;
                case nameof(testedMedia.CmiData):
                    buffer        = testedMedia.CmiData;
                    model.Decoded = CSS_CPRM.PrettifyLeadInCopyright(buffer);

                    break;
                case nameof(testedMedia.CorrectedSubchannelData):
                    buffer = testedMedia.CorrectedSubchannelData;

                    if(buffer is null       ||
                       buffer.Length < 2352 ||
                       buffer.All(c => c == 0))
                        break;

                    Array.Copy(buffer, 0, sector, 0, 2352);

                    model.Decoded = Sector.Prettify(sector);

                    if(buffer.Length < 2448)
                        break;

                    Array.Copy(buffer, 2352, fullsub, 0, 96);

                    model.Decoded += "\n" + GetPrettySub(fullsub);

                    break;
                case nameof(testedMedia.CorrectedSubchannelWithC2Data):
                    buffer = testedMedia.CorrectedSubchannelWithC2Data;

                    if(buffer is null       ||
                       buffer.Length < 2352 ||
                       buffer.All(c => c == 0))
                        break;

                    Array.Copy(buffer, 0, sector, 0, 2352);

                    model.Decoded = Sector.Prettify(sector);

                    if(buffer.Length < 2448)
                        break;

                    for(int i = 2352; i < 2616; i++)
                    {
                        if(buffer[i] == 0x00)
                            continue;

                        c2Errors = true;

                        break;
                    }

                    model.Decoded += "\n" + (c2Errors ? "C2 errors found." : "No C2 errors.");

                    break;
                case nameof(testedMedia.DcbData):
                    buffer = testedMedia.DcbData;

                    break;
                case nameof(testedMedia.DmiData):
                    buffer = testedMedia.DmiData;

                    if(DMI.IsXbox(buffer))
                        model.Decoded = DMI.PrettifyXbox(buffer);
                    else if(DMI.IsXbox360(buffer))
                        model.Decoded = DMI.PrettifyXbox360(buffer);

                    break;
                case nameof(testedMedia.DvdAacsData):
                    buffer = testedMedia.DvdAacsData;

                    break;
                case nameof(testedMedia.DvdBcaData):
                    buffer = testedMedia.DvdBcaData;

                    break;
                case nameof(testedMedia.DvdDdsData):
                    buffer        = testedMedia.DvdDdsData;
                    model.Decoded = Decoders.DVD.DDS.Prettify(buffer);

                    break;
                case nameof(testedMedia.DvdLayerData):
                    buffer = testedMedia.DvdLayerData;

                    break;
                case nameof(testedMedia.DvdSaiData):
                    buffer        = testedMedia.DvdSaiData;
                    model.Decoded = Decoders.DVD.Spare.Prettify(buffer);

                    break;
                case nameof(testedMedia.EmbossedPfiData):
                    buffer        = testedMedia.EmbossedPfiData;
                    model.Decoded = PFI.Prettify(buffer, MediaType.DVDROM); // TODO: Get real media type here

                    break;
                case nameof(testedMedia.FullTocData):
                    buffer        = testedMedia.FullTocData;
                    model.Decoded = FullTOC.Prettify(buffer);

                    break;
                case nameof(testedMedia.HdCmiData):
                    buffer = testedMedia.HdCmiData;

                    break;
                case nameof(testedMedia.HLDTSTReadRawDVDData):
                    buffer = testedMedia.HLDTSTReadRawDVDData;

                    break;
                case nameof(testedMedia.IdentifyData):
                    buffer        = testedMedia.IdentifyData;
                    model.Decoded = Identify.Prettify(buffer);

                    break;
                case nameof(testedMedia.IntersessionLeadInData):
                    buffer = testedMedia.IntersessionLeadInData;

                    break;
                case nameof(testedMedia.IntersessionLeadOutData):
                    buffer = testedMedia.IntersessionLeadOutData;

                    break;
                case nameof(testedMedia.LeadInData):
                    buffer        = testedMedia.LeadInData;
                    model.Decoded = Sector.Prettify(buffer);

                    break;
                case nameof(testedMedia.LeadOutData):
                    buffer        = testedMedia.LeadOutData;
                    model.Decoded = Sector.Prettify(buffer);

                    break;
                case nameof(testedMedia.ModeSense6Data):
                    buffer        = testedMedia.ModeSense6Data;
                    model.Decoded = Modes.PrettifyModeHeader6(buffer, PeripheralDeviceTypes.DirectAccess);

                    break;
                case nameof(testedMedia.ModeSense10Data):
                    buffer        = testedMedia.ModeSense10Data;
                    model.Decoded = Modes.PrettifyModeHeader10(buffer, PeripheralDeviceTypes.DirectAccess);

                    break;
                case nameof(testedMedia.NecReadCddaData):
                    buffer = testedMedia.NecReadCddaData;

                    break;
                case nameof(testedMedia.PfiData):
                    buffer        = testedMedia.PfiData;
                    model.Decoded = PFI.Prettify(buffer, MediaType.DVDROM); // TODO: Get real media type here

                    break;
                case nameof(testedMedia.PioneerReadCddaData):
                    buffer = testedMedia.PioneerReadCddaData;

                    break;
                case nameof(testedMedia.PioneerReadCddaMsfData):
                    buffer = testedMedia.PioneerReadCddaMsfData;

                    break;
                case nameof(testedMedia.PlextorReadCddaData):
                    buffer = testedMedia.PlextorReadCddaData;

                    break;
                case nameof(testedMedia.PlextorReadRawDVDData):
                    buffer = testedMedia.PlextorReadRawDVDData;

                    break;
                case nameof(testedMedia.PmaData):
                    buffer        = testedMedia.PmaData;
                    model.Decoded = PMA.Prettify(buffer);

                    break;
                case nameof(testedMedia.PQSubchannelData):
                    buffer = testedMedia.PQSubchannelData;

                    if(buffer is null       ||
                       buffer.Length < 2352 ||
                       buffer.All(c => c == 0))
                        break;

                    Array.Copy(buffer, 0, sector, 0, 2352);

                    model.Decoded = Sector.Prettify(sector);

                    if(buffer.Length < 2368)
                        break;

                    Array.Copy(buffer, 2352, subq, 0, 16);
                    fullsub = Subchannel.ConvertQToRaw(subq);

                    model.Decoded += "\n" + GetPrettySub(fullsub);

                    break;
                case nameof(testedMedia.PQSubchannelWithC2Data):
                    buffer = testedMedia.PQSubchannelWithC2Data;

                    if(buffer is null       ||
                       buffer.Length < 2352 ||
                       buffer.All(c => c == 0))
                        break;

                    Array.Copy(buffer, 0, sector, 0, 2352);

                    model.Decoded = Sector.Prettify(sector);

                    if(buffer.Length < 2368)
                        break;

                    Array.Copy(buffer, 2646, subq, 0, 16);
                    fullsub = Subchannel.ConvertQToRaw(subq);

                    model.Decoded += "\n" + GetPrettySub(fullsub);

                    for(int i = 2352; i < 2646; i++)
                    {
                        if(buffer[i] == 0x00)
                            continue;

                        c2Errors = true;

                        break;
                    }

                    model.Decoded += "\n" + (c2Errors ? "C2 errors found." : "No C2 errors.");

                    break;
                case nameof(testedMedia.PriData):
                    buffer = testedMedia.PriData;

                    break;
                case nameof(testedMedia.Read6Data):
                    buffer = testedMedia.Read6Data;

                    break;
                case nameof(testedMedia.Read10Data):
                    buffer = testedMedia.Read10Data;

                    break;
                case nameof(testedMedia.Read12Data):
                    buffer = testedMedia.Read12Data;

                    break;
                case nameof(testedMedia.Read16Data):
                    buffer = testedMedia.Read16Data;

                    break;
                case nameof(testedMedia.ReadCdData):
                    buffer = testedMedia.ReadCdData;

                    break;
                case nameof(testedMedia.ReadCdFullData):
                    buffer        = testedMedia.ReadCdFullData;
                    model.Decoded = Sector.Prettify(buffer);

                    break;
                case nameof(testedMedia.ReadCdMsfData):
                    buffer = testedMedia.ReadCdMsfData;

                    break;
                case nameof(testedMedia.ReadCdMsfFullData):
                    buffer        = testedMedia.ReadCdMsfFullData;
                    model.Decoded = Sector.Prettify(buffer);

                    break;
                case nameof(testedMedia.ReadDmaData):
                    buffer = testedMedia.ReadDmaData;

                    break;
                case nameof(testedMedia.ReadDmaLba48Data):
                    buffer = testedMedia.ReadDmaLba48Data;

                    break;
                case nameof(testedMedia.ReadDmaLbaData):
                    buffer = testedMedia.ReadDmaLbaData;

                    break;
                case nameof(testedMedia.ReadDmaRetryData):
                    buffer = testedMedia.ReadDmaRetryData;

                    break;
                case nameof(testedMedia.ReadDmaRetryLbaData):
                    buffer = testedMedia.ReadDmaRetryLbaData;

                    break;
                case nameof(testedMedia.ReadLba48Data):
                    buffer = testedMedia.ReadLba48Data;

                    break;
                case nameof(testedMedia.ReadLbaData):
                    buffer = testedMedia.ReadLbaData;

                    break;
                case nameof(testedMedia.ReadLong10Data):
                    buffer = testedMedia.ReadLong10Data;

                    break;
                case nameof(testedMedia.ReadLong16Data):
                    buffer = testedMedia.ReadLong16Data;

                    break;
                case nameof(testedMedia.ReadLongData):
                    buffer = testedMedia.ReadLongData;

                    break;
                case nameof(testedMedia.ReadLongLbaData):
                    buffer = testedMedia.ReadLongLbaData;

                    break;
                case nameof(testedMedia.ReadLongRetryData):
                    buffer = testedMedia.ReadLongRetryData;

                    break;
                case nameof(testedMedia.ReadLongRetryLbaData):
                    buffer = testedMedia.ReadLongRetryLbaData;

                    break;
                case nameof(testedMedia.ReadRetryLbaData):
                    buffer = testedMedia.ReadRetryLbaData;

                    break;
                case nameof(testedMedia.ReadSectorsData):
                    buffer = testedMedia.ReadSectorsData;

                    break;
                case nameof(testedMedia.ReadSectorsRetryData):
                    buffer = testedMedia.ReadSectorsRetryData;

                    break;
                case nameof(testedMedia.RWSubchannelData):
                    buffer = testedMedia.RWSubchannelData;

                    if(buffer is null       ||
                       buffer.Length < 2352 ||
                       buffer.All(c => c == 0))
                        break;

                    Array.Copy(buffer, 0, sector, 0, 2352);

                    model.Decoded = Sector.Prettify(sector);

                    if(buffer.Length < 2448)
                        break;

                    Array.Copy(buffer, 2352, fullsub, 0, 96);

                    model.Decoded += "\n" + GetPrettySub(fullsub);

                    break;
                case nameof(testedMedia.RWSubchannelWithC2Data):
                    buffer = testedMedia.RWSubchannelWithC2Data;

                    if(buffer is null       ||
                       buffer.Length < 2352 ||
                       buffer.All(c => c == 0))
                        break;

                    Array.Copy(buffer, 0, sector, 0, 2352);

                    model.Decoded = Sector.Prettify(sector);

                    if(buffer.Length < 2448)
                        break;

                    Array.Copy(buffer, 2352, fullsub, 0, 96);

                    model.Decoded += "\n" + GetPrettySub(fullsub);

                    for(int i = 2448; i < buffer.Length; i++)
                    {
                        if(buffer[i] != 0x00)
                        {
                            c2Errors = true;

                            break;
                        }
                    }

                    model.Decoded += "\n" + (c2Errors ? "C2 errors found." : "No C2 errors.");

                    break;
                case nameof(testedMedia.TocData):
                    buffer        = testedMedia.TocData;
                    model.Decoded = TOC.Prettify(buffer);

                    break;
                case nameof(testedMedia.Track1PregapData):
                    buffer = testedMedia.Track1PregapData;

                    model.Decoded = Sector.Prettify(buffer);

                    break;
                case nameof(testedMedia.ReadCdScrambledData):
                    buffer = testedMedia.ReadCdScrambledData;

                    break;
                case nameof(testedMedia.ReadF1_06Data):
                    buffer = testedMedia.ReadF1_06Data;

                    if(buffer.Length != 0xB00)
                    {
                        model.Decoded = Sense.PrettifySense(buffer);

                        break;
                    }

                    Array.Copy(buffer, 0, sector, 0, 2352);

                    if((sector[0xD] & 0x80) == 0x80)
                    {
                        scrambled = true;
                        Sector.Scramble(sector);
                    }

                    model.Decoded = Sector.Prettify(sector) + "\n" + (scrambled ? "Scrambled." : "Descrambled.");

                    Array.Copy(buffer, 2352, fullsub, 0, 96);

                    model.Decoded += "\n" + GetPrettySub(fullsub);

                    Array.Copy(buffer, 2448, subq, 0, 16);
                    fullsub = Subchannel.ConvertQToRaw(subq);

                    model.Decoded += "\n" + GetPrettySub(fullsub);

                    for(int i = 2468; i < 2762; i++)
                    {
                        if(buffer[i] == 0x00)
                            continue;

                        c2Errors = true;

                        break;
                    }

                    model.Decoded += "\n" + (c2Errors ? "C2 errors found." : "No C2 errors.");

                    break;
                case nameof(testedMedia.ReadF1_06LeadOutData):
                    buffer = testedMedia.ReadF1_06LeadOutData;

                    if(buffer.Length != 0xB00)
                    {
                        model.Decoded = Sense.PrettifySense(buffer);

                        break;
                    }

                    Array.Copy(buffer, 0, sector, 0, 2352);

                    if((sector[0xD] & 0x80) == 0x80)
                    {
                        scrambled = true;
                        Sector.Scramble(sector);
                    }

                    model.Decoded = Sector.Prettify(sector) + "\n" + (scrambled ? "Scrambled." : "Descrambled.");

                    Array.Copy(buffer, 2352, fullsub, 0, 96);

                    model.Decoded += "\n" + GetPrettySub(fullsub);

                    Array.Copy(buffer, 2448, subq, 0, 16);
                    fullsub = Subchannel.ConvertQToRaw(subq);

                    model.Decoded += "\n" + GetPrettySub(fullsub);

                    for(int i = 2468; i < 2762; i++)
                    {
                        if(buffer[i] == 0x00)
                            continue;

                        c2Errors = true;

                        break;
                    }

                    model.Decoded += "\n" + (c2Errors ? "C2 errors found." : "No C2 errors.");

                    break;
                default: return NotFound();
            }

            model.RawDataAsHex = PrintHex.ByteArrayToHexArrayString(buffer);

            if(model.RawDataAsHex != null)
                model.RawDataAsHex = HttpUtility.HtmlEncode(model.RawDataAsHex).Replace("\n", "<br/>");

            if(model.Decoded != null)
                model.Decoded = HttpUtility.HtmlEncode(model.Decoded).Replace("\n", "<br/>");

            return View(model);
        }

        static string GetPrettySub(byte[] sub)
        {
            byte[] deint = Subchannel.Deinterleave(sub);

            bool validP  = true;
            bool validRw = true;

            for(int i = 0; i < 12; i++)
            {
                if(deint[i] == 0x00 ||
                   deint[i] == 0xFF)
                    continue;

                validP = false;

                break;
            }

            for(int i = 24; i < 96; i++)
            {
                if(deint[i] == 0x00)
                    continue;

                validRw = false;

                break;
            }

            byte[] q = new byte[12];
            Array.Copy(deint, 12, q, 0, 12);

            return Subchannel.PrettifyQ(q, deint[21] > 0x10, 16, !validP, false, validRw);
        }
    }
}