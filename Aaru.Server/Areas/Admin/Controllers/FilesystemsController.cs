using System.Linq;
using System.Threading.Tasks;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class FilesystemsController : Controller
    {
        readonly DicServerContext _context;

        public FilesystemsController(DicServerContext context) => _context = context;

        // GET: Admin/Filesystems
        public async Task<IActionResult> Index() => View(await _context.Filesystems.OrderBy(f => f.Name).ToListAsync());
    }
}