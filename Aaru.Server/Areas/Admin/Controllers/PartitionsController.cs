using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class PartitionsController : Controller
    {
        readonly DicServerContext _context;

        public PartitionsController(DicServerContext context) => _context = context;

        // GET: Admin/Partitions
        public async Task<IActionResult> Index() => View(await _context.Partitions.OrderBy(p => p.Name).ToListAsync());
    }
}