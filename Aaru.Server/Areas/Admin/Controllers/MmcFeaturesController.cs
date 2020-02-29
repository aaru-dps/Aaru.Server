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
    }
}