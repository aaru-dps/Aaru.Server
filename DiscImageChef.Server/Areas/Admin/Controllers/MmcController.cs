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
    public class MmcController : Controller
    {
        readonly DicServerContext _context;

        public MmcController(DicServerContext context) => _context = context;

        // GET: Admin/Mmc
        public IActionResult Index() => View(_context.Mmc.Where(m => m.ModeSense2AData != null).
                                                      Select(m => new MmcModelForView
                                                      {
                                                          Id         = m.Id, FeaturesId = m.FeaturesId,
                                                          DataLength = m.ModeSense2AData.Length
                                                      }).ToList().
                                                      Concat(_context.Mmc.Where(m => m.ModeSense2AData == null).
                                                                      Select(m => new MmcModelForView
                                                                      {
                                                                          Id         = m.Id, FeaturesId = m.FeaturesId,
                                                                          DataLength = 0
                                                                      }).ToList()).OrderBy(m => m.Id));

        // GET: Admin/Mmc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Mmc mmc = await _context.Mmc.FirstOrDefaultAsync(m => m.Id == id);

            if(mmc == null)
            {
                return NotFound();
            }

            return View(mmc);
        }

        // GET: Admin/Mmc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Mmc mmc = await _context.Mmc.FirstOrDefaultAsync(m => m.Id == id);

            if(mmc == null)
            {
                return NotFound();
            }

            return View(mmc);
        }

        // POST: Admin/Mmc/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Mmc         mmc     = await _context.Mmc.FindAsync(id);
            MmcFeatures feature = await _context.MmcFeatures.FirstOrDefaultAsync(f => f.Id == mmc.FeaturesId);

            _context.MmcFeatures.Remove(feature);
            _context.Mmc.Remove(mmc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}