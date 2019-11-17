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
    public class SscsController : Controller
    {
        readonly DicServerContext _context;

        public SscsController(DicServerContext context) => _context = context;

        // GET: Admin/Sscs
        public async Task<IActionResult> Index() => View(await _context.Ssc.ToListAsync());

        // GET: Admin/Sscs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Ssc ssc = await _context.Ssc.FirstOrDefaultAsync(m => m.Id == id);

            if(ssc == null)
            {
                return NotFound();
            }

            return View(ssc);
        }

        // GET: Admin/Sscs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Ssc ssc = await _context.Ssc.FirstOrDefaultAsync(m => m.Id == id);

            if(ssc == null)
            {
                return NotFound();
            }

            return View(ssc);
        }

        // POST: Admin/Sscs/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Ssc ssc = await _context.Ssc.FindAsync(id);
            _context.Ssc.Remove(ssc);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}