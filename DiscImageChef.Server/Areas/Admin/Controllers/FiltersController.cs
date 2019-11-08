using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class FiltersController : Controller
    {
        readonly DicServerContext _context;

        public FiltersController(DicServerContext context) => _context = context;

        // GET: Admin/Filters
        public async Task<IActionResult> Index() => View(await _context.Filters.ToListAsync());

        // GET: Admin/Filters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Filter filter = await _context.Filters.FirstOrDefaultAsync(m => m.Id == id);

            if(filter == null)
            {
                return NotFound();
            }

            return View(filter);
        }

        // GET: Admin/Filters/Create
        public IActionResult Create() => View();

        // POST: Admin/Filters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Count")] Filter filter)
        {
            if(ModelState.IsValid)
            {
                _context.Add(filter);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(filter);
        }

        // GET: Admin/Filters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Filter filter = await _context.Filters.FindAsync(id);

            if(filter == null)
            {
                return NotFound();
            }

            return View(filter);
        }

        // POST: Admin/Filters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Count")] Filter filter)
        {
            if(id != filter.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(filter);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!FilterExists(filter.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(filter);
        }

        // GET: Admin/Filters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Filter filter = await _context.Filters.FirstOrDefaultAsync(m => m.Id == id);

            if(filter == null)
            {
                return NotFound();
            }

            return View(filter);
        }

        // POST: Admin/Filters/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Filter filter = await _context.Filters.FindAsync(id);
            _context.Filters.Remove(filter);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool FilterExists(int id) => _context.Filters.Any(e => e.Id == id);
    }
}