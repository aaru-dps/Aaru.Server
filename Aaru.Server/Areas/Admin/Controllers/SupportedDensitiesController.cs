using System.Linq;
using System.Threading.Tasks;
using Aaru.CommonTypes.Metadata;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public sealed class SupportedDensitiesController : Controller
    {
        readonly AaruServerContext _context;

        public SupportedDensitiesController(AaruServerContext context) => _context = context;

        // GET: Admin/SupportedDensities
        public async Task<IActionResult> Index() => View(await _context.SupportedDensity.OrderBy(d => d.Organization).
                                                                        ThenBy(d => d.Name).ThenBy(d => d.Description).
                                                                        ThenBy(d => d.Capacity).
                                                                        ThenBy(d => d.PrimaryCode).
                                                                        ThenBy(d => d.SecondaryCode).
                                                                        ThenBy(d => d.BitsPerMm).ThenBy(d => d.Width).
                                                                        ThenBy(d => d.Tracks).
                                                                        ThenBy(d => d.DefaultDensity).
                                                                        ThenBy(d => d.Writable).
                                                                        ThenBy(d => d.Duplicate).ToListAsync());

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