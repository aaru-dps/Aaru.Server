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
    public class ScsiModesController : Controller
    {
        readonly DicServerContext _context;

        public ScsiModesController(DicServerContext context) => _context = context;

        // GET: Admin/ScsiModes
        public async Task<IActionResult> Index() => View(await _context.ScsiMode.ToListAsync());

        // GET: Admin/ScsiModes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ScsiMode scsiMode = await _context.ScsiMode.FirstOrDefaultAsync(m => m.Id == id);

            if(scsiMode == null)
            {
                return NotFound();
            }

            return View(scsiMode);
        }

        // GET: Admin/ScsiModes/Create
        public IActionResult Create() => View();

        // POST: Admin/ScsiModes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,MediumType,WriteProtected,Speed,BufferedMode,BlankCheckEnabled,DPOandFUA")]
            ScsiMode scsiMode)
        {
            if(ModelState.IsValid)
            {
                _context.Add(scsiMode);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(scsiMode);
        }

        // GET: Admin/ScsiModes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ScsiMode scsiMode = await _context.ScsiMode.FindAsync(id);

            if(scsiMode == null)
            {
                return NotFound();
            }

            return View(scsiMode);
        }

        // POST: Admin/ScsiModes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("Id,MediumType,WriteProtected,Speed,BufferedMode,BlankCheckEnabled,DPOandFUA")]
            ScsiMode scsiMode)
        {
            if(id != scsiMode.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(scsiMode);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!ScsiModeExists(scsiMode.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(scsiMode);
        }

        // GET: Admin/ScsiModes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ScsiMode scsiMode = await _context.ScsiMode.FirstOrDefaultAsync(m => m.Id == id);

            if(scsiMode == null)
            {
                return NotFound();
            }

            return View(scsiMode);
        }

        // POST: Admin/ScsiModes/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ScsiMode scsiMode = await _context.ScsiMode.FindAsync(id);
            _context.ScsiMode.Remove(scsiMode);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ScsiModeExists(int id) => _context.ScsiMode.Any(e => e.Id == id);
    }
}