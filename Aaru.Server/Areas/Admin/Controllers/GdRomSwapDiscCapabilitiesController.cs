using System.Text;
using System.Web;
using Aaru.CommonTypes.Metadata;
using Aaru.Decoders.CD;
using Aaru.Helpers;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class GdRomSwapDiscCapabilitiesController : Controller
{
    readonly AaruServerContext _context;

    public GdRomSwapDiscCapabilitiesController(AaruServerContext context) => _context = context;

    // GET: Admin/GdRomSwapDiscCapabilities/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if(id == null)
        {
            return NotFound();
        }

        GdRomSwapDiscCapabilities caps = await _context.GdRomSwapDiscCapabilities.FirstOrDefaultAsync(m => m.Id == id);

        if(caps == null)
        {
            return NotFound();
        }

        return View(caps);
    }

    // GET: Admin/GdRomSwapDiscCapabilities/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if(id == null)
        {
            return NotFound();
        }

        GdRomSwapDiscCapabilities caps = await _context.GdRomSwapDiscCapabilities.FirstOrDefaultAsync(m => m.Id == id);

        if(caps == null)
        {
            return NotFound();
        }

        return View(caps);
    }

    // POST: Admin/GdRomSwapDiscCapabilities/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        GdRomSwapDiscCapabilities caps = await _context.GdRomSwapDiscCapabilities.FindAsync(id);
        _context.GdRomSwapDiscCapabilities.Remove(caps);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Admin");
    }

    public IActionResult ViewData(int id, string data)
    {
        if(string.IsNullOrWhiteSpace(data))
            return NotFound();

        GdRomSwapDiscCapabilities caps = _context.GdRomSwapDiscCapabilities.FirstOrDefault(m => m.Id == id);

        if(caps == null)
        {
            return NotFound();
        }

        var model = new TestedMediaDataModel
        {
            TestedMediaId = id,
            DataName      = data
        };

        byte[] buffer;
        var    sb      = new StringBuilder();
        byte[] sector  = new byte[2352];
        byte[] subq    = new byte[16];
        byte[] fullsub = new byte[96];

        bool audio = true;
        bool pq    = false;
        bool rw    = false;

        switch(data)
        {
            case nameof(caps.Lba0Data):
                buffer = caps.Lba0Data;
                audio  = false;

                break;
            case nameof(caps.Lba0ScrambledData):
                buffer = caps.Lba0ScrambledData;

                break;
            case nameof(caps.Lba44990Data):
                buffer = caps.Lba44990Data;
                audio  = false;

                break;
            case nameof(caps.Lba44990PqData):
                buffer = caps.Lba44990PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba44990RwData):
                buffer = caps.Lba44990RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba44990AudioData):
                buffer = caps.Lba44990AudioData;

                break;
            case nameof(caps.Lba44990AudioPqData):
                buffer = caps.Lba44990AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba44990AudioRwData):
                buffer = caps.Lba44990AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba45000Data):
                buffer = caps.Lba45000Data;
                audio  = false;

                break;
            case nameof(caps.Lba45000PqData):
                buffer = caps.Lba45000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba45000RwData):
                buffer = caps.Lba45000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba45000AudioData):
                buffer = caps.Lba45000AudioData;

                break;
            case nameof(caps.Lba45000AudioPqData):
                buffer = caps.Lba45000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba45000AudioRwData):
                buffer = caps.Lba45000AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba50000Data):
                buffer = caps.Lba50000Data;
                audio  = false;

                break;
            case nameof(caps.Lba50000PqData):
                buffer = caps.Lba50000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba50000RwData):
                buffer = caps.Lba50000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba50000AudioData):
                buffer = caps.Lba50000AudioData;

                break;
            case nameof(caps.Lba50000AudioPqData):
                buffer = caps.Lba50000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba50000AudioRwData):
                buffer = caps.Lba50000AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba100000Data):
                buffer = caps.Lba100000Data;
                audio  = false;

                break;
            case nameof(caps.Lba100000PqData):
                buffer = caps.Lba100000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba100000RwData):
                buffer = caps.Lba100000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba100000AudioData):
                buffer = caps.Lba100000AudioData;

                break;
            case nameof(caps.Lba100000AudioPqData):
                buffer = caps.Lba100000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba100000AudioRwData):
                buffer = caps.Lba100000AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba400000Data):
                buffer = caps.Lba400000Data;
                audio  = false;

                break;
            case nameof(caps.Lba400000PqData):
                buffer = caps.Lba400000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba400000RwData):
                buffer = caps.Lba400000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba400000AudioData):
                buffer = caps.Lba400000AudioData;

                break;
            case nameof(caps.Lba400000AudioPqData):
                buffer = caps.Lba400000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba400000AudioRwData):
                buffer = caps.Lba400000AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba450000Data):
                buffer = caps.Lba450000Data;
                audio  = false;

                break;
            case nameof(caps.Lba450000PqData):
                buffer = caps.Lba450000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba450000RwData):
                buffer = caps.Lba450000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba450000AudioData):
                buffer = caps.Lba450000AudioData;

                break;
            case nameof(caps.Lba450000AudioPqData):
                buffer = caps.Lba450000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba450000AudioRwData):
                buffer = caps.Lba450000AudioRwData;
                rw     = true;

                break;
            default: return NotFound();
        }

        if(pq                           &&
           buffer               != null &&
           buffer.Length % 2368 != 0)
            pq = false;

        if(rw                           &&
           buffer               != null &&
           buffer.Length % 2448 != 0)
            rw = false;

        int blockSize = pq
                            ? 2368
                            : rw
                                ? 2448
                                : 2352;

        model.RawDataAsHex = PrintHex.ByteArrayToHexArrayString(buffer);

        if(model.RawDataAsHex != null)
            model.RawDataAsHex = HttpUtility.HtmlEncode(model.RawDataAsHex).Replace("\n", "<br/>");

        if(buffer == null)
            return View(model);

        for(int i = 0; i < buffer.Length; i += blockSize)
        {
            if(audio)
            {
                sb.AppendLine("Audio or scrambled data sector.");
            }
            else
            {
                Array.Copy(buffer, i, sector, 0, 2352);

                sb.AppendLine(Sector.Prettify(sector));
            }

            if(pq)
            {
                Array.Copy(buffer, i + 2352, subq, 0, 16);
                fullsub = Subchannel.ConvertQToRaw(subq);

                sb.AppendLine(GetPrettySub(fullsub));
            }
            else if(rw)
            {
                Array.Copy(buffer, i + 2352, fullsub, 0, 96);

                sb.AppendLine(GetPrettySub(fullsub));
            }

            sb.AppendLine();
        }

        model.Decoded = HttpUtility.HtmlEncode(sb.ToString()).Replace("\n", "<br/>");

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