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
    public class SupportedDensitiesController : Controller
    {
        readonly DicServerContext _context;

        public SupportedDensitiesController(DicServerContext context) => _context = context;

        // GET: Admin/SupportedDensities
        public async Task<IActionResult> Index() => View(await _context.SupportedDensity.ToListAsync());

        // GET: Admin/SupportedDensities/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Admin/SupportedDensities/Create
        public IActionResult Create() => View();

        // POST: Admin/SupportedDensities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "Id,PrimaryCode,SecondaryCode,Writable,Duplicate,DefaultDensity,BitsPerMm,Width,Tracks,Capacity,Organization,Name,Description")]
            SupportedDensity supportedDensity)
        {
            if(ModelState.IsValid)
            {
                _context.Add(supportedDensity);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(supportedDensity);
        }

        // GET: Admin/SupportedDensities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            SupportedDensity supportedDensity = await _context.SupportedDensity.FindAsync(id);

            if(supportedDensity == null)
            {
                return NotFound();
            }

            return View(supportedDensity);
        }

        // POST: Admin/SupportedDensities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind(
                "Id,PrimaryCode,SecondaryCode,Writable,Duplicate,DefaultDensity,BitsPerMm,Width,Tracks,Capacity,Organization,Name,Description")]
            SupportedDensity supportedDensity)
        {
            if(id != supportedDensity.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(supportedDensity);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!SupportedDensityExists(supportedDensity.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(supportedDensity);
        }

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

        bool SupportedDensityExists(int id) => _context.SupportedDensity.Any(e => e.Id == id);
    }
}