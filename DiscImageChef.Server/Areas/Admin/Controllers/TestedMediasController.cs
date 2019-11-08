using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TestedMediasController : Controller
    {
        private readonly DicServerContext _context;

        public TestedMediasController(DicServerContext context)
        {
            _context = context;
        }

        // GET: Admin/TestedMedias
        public async Task<IActionResult> Index()
        {
            return View(await _context.TestedMedia.ToListAsync());
        }

        // GET: Admin/TestedMedias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testedMedia = await _context.TestedMedia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testedMedia == null)
            {
                return NotFound();
            }

            return View(testedMedia);
        }

        // GET: Admin/TestedMedias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/TestedMedias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdentifyData,Blocks,BlockSize,CanReadAACS,CanReadADIP,CanReadATIP,CanReadBCA,CanReadC2Pointers,CanReadCMI,CanReadCorrectedSubchannel,CanReadCorrectedSubchannelWithC2,CanReadDCB,CanReadDDS,CanReadDMI,CanReadDiscInformation,CanReadFullTOC,CanReadHDCMI,CanReadLayerCapacity,CanReadFirstTrackPreGap,CanReadLeadIn,CanReadLeadOut,CanReadMediaID,CanReadMediaSerial,CanReadPAC,CanReadPFI,CanReadPMA,CanReadPQSubchannel,CanReadPQSubchannelWithC2,CanReadPRI,CanReadRWSubchannel,CanReadRWSubchannelWithC2,CanReadRecordablePFI,CanReadSpareAreaInformation,CanReadTOC,Density,LongBlockSize,Manufacturer,MediaIsRecognized,MediumType,MediumTypeName,Model,SupportsHLDTSTReadRawDVD,SupportsNECReadCDDA,SupportsPioneerReadCDDA,SupportsPioneerReadCDDAMSF,SupportsPlextorReadCDDA,SupportsPlextorReadRawDVD,SupportsRead10,SupportsRead12,SupportsRead16,SupportsRead6,SupportsReadCapacity16,SupportsReadCapacity,SupportsReadCd,SupportsReadCdMsf,SupportsReadCdRaw,SupportsReadCdMsfRaw,SupportsReadLong16,SupportsReadLong,ModeSense6Data,ModeSense10Data,LBASectors,LBA48Sectors,LogicalAlignment,NominalRotationRate,PhysicalBlockSize,SolidStateDevice,UnformattedBPT,UnformattedBPS,SupportsReadDmaLba,SupportsReadDmaRetryLba,SupportsReadLba,SupportsReadRetryLba,SupportsReadLongLba,SupportsReadLongRetryLba,SupportsSeekLba,SupportsReadDmaLba48,SupportsReadLba48,SupportsReadDma,SupportsReadDmaRetry,SupportsReadRetry,SupportsReadSectors,SupportsReadLongRetry,SupportsSeek,CanReadingIntersessionLeadIn,CanReadingIntersessionLeadOut,IntersessionLeadInData,IntersessionLeadOutData,Read6Data,Read10Data,Read12Data,Read16Data,ReadLong10Data,ReadLong16Data,ReadSectorsData,ReadSectorsRetryData,ReadDmaData,ReadDmaRetryData,ReadLbaData,ReadRetryLbaData,ReadDmaLbaData,ReadDmaRetryLbaData,ReadLba48Data,ReadDmaLba48Data,ReadLongData,ReadLongRetryData,ReadLongLbaData,ReadLongRetryLbaData,TocData,FullTocData,AtipData,PmaData,ReadCdData,ReadCdMsfData,ReadCdFullData,ReadCdMsfFullData,Track1PregapData,LeadInData,LeadOutData,C2PointersData,PQSubchannelData,RWSubchannelData,CorrectedSubchannelData,PQSubchannelWithC2Data,RWSubchannelWithC2Data,CorrectedSubchannelWithC2Data,PfiData,DmiData,CmiData,DvdBcaData,DvdAacsData,DvdDdsData,DvdSaiData,PriData,EmbossedPfiData,AdipData,DcbData,HdCmiData,DvdLayerData,BluBcaData,BluDdsData,BluSaiData,BluDiData,BluPacData,PlextorReadCddaData,PioneerReadCddaData,PioneerReadCddaMsfData,NecReadCddaData,PlextorReadRawDVDData,HLDTSTReadRawDVDData")] TestedMedia testedMedia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testedMedia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testedMedia);
        }

        // GET: Admin/TestedMedias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testedMedia = await _context.TestedMedia.FindAsync(id);
            if (testedMedia == null)
            {
                return NotFound();
            }
            return View(testedMedia);
        }

        // POST: Admin/TestedMedias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdentifyData,Blocks,BlockSize,CanReadAACS,CanReadADIP,CanReadATIP,CanReadBCA,CanReadC2Pointers,CanReadCMI,CanReadCorrectedSubchannel,CanReadCorrectedSubchannelWithC2,CanReadDCB,CanReadDDS,CanReadDMI,CanReadDiscInformation,CanReadFullTOC,CanReadHDCMI,CanReadLayerCapacity,CanReadFirstTrackPreGap,CanReadLeadIn,CanReadLeadOut,CanReadMediaID,CanReadMediaSerial,CanReadPAC,CanReadPFI,CanReadPMA,CanReadPQSubchannel,CanReadPQSubchannelWithC2,CanReadPRI,CanReadRWSubchannel,CanReadRWSubchannelWithC2,CanReadRecordablePFI,CanReadSpareAreaInformation,CanReadTOC,Density,LongBlockSize,Manufacturer,MediaIsRecognized,MediumType,MediumTypeName,Model,SupportsHLDTSTReadRawDVD,SupportsNECReadCDDA,SupportsPioneerReadCDDA,SupportsPioneerReadCDDAMSF,SupportsPlextorReadCDDA,SupportsPlextorReadRawDVD,SupportsRead10,SupportsRead12,SupportsRead16,SupportsRead6,SupportsReadCapacity16,SupportsReadCapacity,SupportsReadCd,SupportsReadCdMsf,SupportsReadCdRaw,SupportsReadCdMsfRaw,SupportsReadLong16,SupportsReadLong,ModeSense6Data,ModeSense10Data,LBASectors,LBA48Sectors,LogicalAlignment,NominalRotationRate,PhysicalBlockSize,SolidStateDevice,UnformattedBPT,UnformattedBPS,SupportsReadDmaLba,SupportsReadDmaRetryLba,SupportsReadLba,SupportsReadRetryLba,SupportsReadLongLba,SupportsReadLongRetryLba,SupportsSeekLba,SupportsReadDmaLba48,SupportsReadLba48,SupportsReadDma,SupportsReadDmaRetry,SupportsReadRetry,SupportsReadSectors,SupportsReadLongRetry,SupportsSeek,CanReadingIntersessionLeadIn,CanReadingIntersessionLeadOut,IntersessionLeadInData,IntersessionLeadOutData,Read6Data,Read10Data,Read12Data,Read16Data,ReadLong10Data,ReadLong16Data,ReadSectorsData,ReadSectorsRetryData,ReadDmaData,ReadDmaRetryData,ReadLbaData,ReadRetryLbaData,ReadDmaLbaData,ReadDmaRetryLbaData,ReadLba48Data,ReadDmaLba48Data,ReadLongData,ReadLongRetryData,ReadLongLbaData,ReadLongRetryLbaData,TocData,FullTocData,AtipData,PmaData,ReadCdData,ReadCdMsfData,ReadCdFullData,ReadCdMsfFullData,Track1PregapData,LeadInData,LeadOutData,C2PointersData,PQSubchannelData,RWSubchannelData,CorrectedSubchannelData,PQSubchannelWithC2Data,RWSubchannelWithC2Data,CorrectedSubchannelWithC2Data,PfiData,DmiData,CmiData,DvdBcaData,DvdAacsData,DvdDdsData,DvdSaiData,PriData,EmbossedPfiData,AdipData,DcbData,HdCmiData,DvdLayerData,BluBcaData,BluDdsData,BluSaiData,BluDiData,BluPacData,PlextorReadCddaData,PioneerReadCddaData,PioneerReadCddaMsfData,NecReadCddaData,PlextorReadRawDVDData,HLDTSTReadRawDVDData")] TestedMedia testedMedia)
        {
            if (id != testedMedia.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testedMedia);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestedMediaExists(testedMedia.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(testedMedia);
        }

        // GET: Admin/TestedMedias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testedMedia = await _context.TestedMedia
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testedMedia == null)
            {
                return NotFound();
            }

            return View(testedMedia);
        }

        // POST: Admin/TestedMedias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testedMedia = await _context.TestedMedia.FindAsync(id);
            _context.TestedMedia.Remove(testedMedia);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestedMediaExists(int id)
        {
            return _context.TestedMedia.Any(e => e.Id == id);
        }
    }
}
