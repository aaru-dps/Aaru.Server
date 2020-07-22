using System.Threading.Tasks;
using Aaru.CommonTypes.Metadata;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public sealed class MmcSdsController : Controller
    {
        readonly AaruServerContext _context;

        public MmcSdsController(AaruServerContext context) => _context = context;

        // GET: Admin/MmcSds
        public async Task<IActionResult> Index() => View(await _context.MmcSd.ToListAsync());

        // GET: Admin/MmcSds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            MmcSd mmcSd = await _context.MmcSd.FirstOrDefaultAsync(m => m.Id == id);

            if(mmcSd == null)
            {
                return NotFound();
            }

            return View(mmcSd);
        }

        // GET: Admin/MmcSds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            MmcSd mmcSd = await _context.MmcSd.FirstOrDefaultAsync(m => m.Id == id);

            if(mmcSd == null)
            {
                return NotFound();
            }

            return View(mmcSd);
        }

        // POST: Admin/MmcSds/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            MmcSd mmcSd = await _context.MmcSd.FindAsync(id);
            _context.MmcSd.Remove(mmcSd);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}