using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class MmcFeaturesController : Controller
    {
        readonly DicServerContext _context;

        public MmcFeaturesController(DicServerContext context) => _context = context;

        // GET: Admin/MmcFeatures
        public async Task<IActionResult> Index() => View(await _context.MmcFeatures.ToListAsync());

        // GET: Admin/MmcFeatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            MmcFeatures mmcFeatures = await _context.MmcFeatures.FirstOrDefaultAsync(m => m.Id == id);

            if(mmcFeatures == null)
            {
                return NotFound();
            }

            return View(mmcFeatures);
        }

        // GET: Admin/MmcFeatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            MmcFeatures mmcFeatures = await _context.MmcFeatures.FindAsync(id);

            if(mmcFeatures == null)
            {
                return NotFound();
            }

            return View(mmcFeatures);
        }

        // POST: Admin/MmcFeatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(
                                                  "Id,AACSVersion,AGIDs,BindingNonceBlocks,BlocksPerReadableUnit,BufferUnderrunFreeInDVD,BufferUnderrunFreeInSAO,BufferUnderrunFreeInTAO,CanAudioScan,CanEject,CanEraseSector,CanExpandBDRESpareArea,CanFormat,CanFormatBDREWithoutSpare,CanFormatCert,CanFormatFRF,CanFormatQCert,CanFormatRRM,CanGenerateBindingNonce,CanLoad,CanMuteSeparateChannels,CanOverwriteSAOTrack,CanOverwriteTAOTrack,CanPlayCDAudio,CanPseudoOverwriteBDR,CanReadAllDualR,CanReadAllDualRW,CanReadBD,CanReadBDR,CanReadBDRE1,CanReadBDRE2,CanReadBDROM,CanReadBluBCA,CanReadCD,CanReadCDMRW,CanReadCPRM_MKB,CanReadDDCD,CanReadDVD,CanReadDVDPlusMRW,CanReadDVDPlusR,CanReadDVDPlusRDL,CanReadDVDPlusRW,CanReadDVDPlusRWDL,CanReadDriveAACSCertificate,CanReadHDDVD,CanReadHDDVDR,CanReadHDDVDRAM,CanReadLeadInCDText,CanReadOldBDR,CanReadOldBDRE,CanReadOldBDROM,CanReadSpareAreaInformation,CanReportDriveSerial,CanReportMediaSerial,CanTestWriteDDCDR,CanTestWriteDVD,CanTestWriteInSAO,CanTestWriteInTAO,CanUpgradeFirmware,CanWriteBD,CanWriteBDR,CanWriteBDRE1,CanWriteBDRE2,CanWriteBusEncryptedBlocks,CanWriteCDMRW,CanWriteCDRW,CanWriteCDRWCAV,CanWriteCDSAO,CanWriteCDTAO,CanWriteCSSManagedDVD,CanWriteDDCDR,CanWriteDDCDRW,CanWriteDVDPlusMRW,CanWriteDVDPlusR,CanWriteDVDPlusRDL,CanWriteDVDPlusRW,CanWriteDVDPlusRWDL,CanWriteDVDR,CanWriteDVDRDL,CanWriteDVDRW,CanWriteHDDVDR,CanWriteHDDVDRAM,CanWriteOldBDR,CanWriteOldBDRE,CanWritePackedSubchannelInTAO,CanWriteRWSubchannelInSAO,CanWriteRWSubchannelInTAO,CanWriteRaw,CanWriteRawMultiSession,CanWriteRawSubchannelInTAO,ChangerIsSideChangeCapable,ChangerSlots,ChangerSupportsDiscPresent,CPRMVersion,CSSVersion,DBML,DVDMultiRead,EmbeddedChanger,ErrorRecoveryPage,FirmwareDate,LoadingMechanismType,Locked,LogicalBlockSize,MultiRead,PhysicalInterfaceStandardNumber,PreventJumper,SupportsAACS,SupportsBusEncryption,SupportsC2,SupportsCPRM,SupportsCSS,SupportsDAP,SupportsDeviceBusyEvent,SupportsHybridDiscs,SupportsModePage1Ch,SupportsOSSC,SupportsPWP,SupportsSWPP,SupportsSecurDisc,SupportsSeparateVolume,SupportsVCPS,SupportsWriteInhibitDCB,SupportsWriteProtectPAC,VolumeLevels,BinaryData")]
                                              MmcFeatures mmcFeatures)
        {
            if(id != mmcFeatures.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(mmcFeatures);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!MmcFeaturesExists(mmcFeatures.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(mmcFeatures);
        }

        // GET: Admin/MmcFeatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            MmcFeatures mmcFeatures = await _context.MmcFeatures.FirstOrDefaultAsync(m => m.Id == id);

            if(mmcFeatures == null)
            {
                return NotFound();
            }

            return View(mmcFeatures);
        }

        // POST: Admin/MmcFeatures/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            MmcFeatures mmcFeatures = await _context.MmcFeatures.FindAsync(id);
            _context.MmcFeatures.Remove(mmcFeatures);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool MmcFeaturesExists(int id) => _context.MmcFeatures.Any(e => e.Id == id);
    }
}