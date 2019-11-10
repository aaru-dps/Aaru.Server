using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class FilesystemsController : Controller
    {
        readonly DicServerContext _context;

        public FilesystemsController(DicServerContext context) => _context = context;

        // GET: Admin/Filesystems
        public async Task<IActionResult> Index() => View(await _context.Filesystems.ToListAsync());
    }
}