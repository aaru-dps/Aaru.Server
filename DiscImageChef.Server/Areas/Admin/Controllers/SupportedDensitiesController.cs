using System.Threading.Tasks;
using DiscImageChef.CommonTypes.Metadata;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class SupportedDensitiesController : Controller
    {
        readonly DicServerContext _context;

        public SupportedDensitiesController(DicServerContext context) => _context = context;

        // GET: Admin/SupportedDensities
        public async Task<IActionResult> Index() => View(await _context.SupportedDensity.ToListAsync());

        // GET: Admin/SupportedDensities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            SupportedDensity supportedDensity = await _context.SupportedDensity.FirstOrDefaultAsync(m => m.Id == id);

            if(supportedDensity == null)
            {
                return NotFound();
            }

            return View(supportedDensity);
        }

        // POST: Admin/SupportedDensities/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            SupportedDensity supportedDensity = await _context.SupportedDensity.FindAsync(id);
            _context.SupportedDensity.Remove(supportedDensity);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}