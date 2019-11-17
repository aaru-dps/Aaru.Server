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
    public class ScsisController : Controller
    {
        readonly DicServerContext _context;

        public ScsisController(DicServerContext context) => _context = context;

        // GET: Admin/Scsis
        public async Task<IActionResult> Index() => View(await _context.Scsi.ToListAsync());

        // GET: Admin/Scsis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Scsi scsi = await _context.Scsi.FirstOrDefaultAsync(m => m.Id == id);

            if(scsi == null)
            {
                return NotFound();
            }

            return View(scsi);
        }

        // GET: Admin/Scsis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Scsi scsi = await _context.Scsi.FirstOrDefaultAsync(m => m.Id == id);

            if(scsi == null)
            {
                return NotFound();
            }

            return View(scsi);
        }

        // POST: Admin/Scsis/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Scsi scsi = await _context.Scsi.FindAsync(id);
            _context.Scsi.Remove(scsi);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ScsiExists(int id) => _context.Scsi.Any(e => e.Id == id);
    }
}