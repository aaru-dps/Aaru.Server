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
    public class ScsiPagesController : Controller
    {
        readonly DicServerContext _context;

        public ScsiPagesController(DicServerContext context) => _context = context;

        // GET: Admin/ScsiPages
        public async Task<IActionResult> Index() => View(await _context.ScsiPage.ToListAsync());

        // GET: Admin/ScsiPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ScsiPage scsiPage = await _context.ScsiPage.FirstOrDefaultAsync(m => m.Id == id);

            if(scsiPage == null)
            {
                return NotFound();
            }

            return View(scsiPage);
        }

        // POST: Admin/ScsiPages/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ScsiPage scsiPage = await _context.ScsiPage.FindAsync(id);
            _context.ScsiPage.Remove(scsiPage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}