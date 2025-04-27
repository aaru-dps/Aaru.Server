using Aaru.CommonTypes.Metadata;
using DbContext = Aaru.Server.Old.Database.DbContext;

namespace Aaru.Server.Old.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class MmcFeaturesController : Controller
{
    readonly DbContext _context;

    public MmcFeaturesController(DbContext context) => _context = context;

    // GET: Admin/MmcFeatures
    public async Task<IActionResult> Index() => View(await _context.MmcFeatures.ToListAsync());

    // GET: Admin/MmcFeatures/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if(id == null) return NotFound();

        MmcFeatures mmcFeatures = await _context.MmcFeatures.FirstOrDefaultAsync(m => m.Id == id);

        if(mmcFeatures == null) return NotFound();

        return View(mmcFeatures);
    }
}