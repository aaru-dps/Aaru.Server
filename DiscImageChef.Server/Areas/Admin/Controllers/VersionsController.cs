using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class VersionsController : Controller
    {
        readonly DicServerContext _context;

        public VersionsController(DicServerContext context) => _context = context;

        // GET: Admin/Versions
        public async Task<IActionResult> Index() => View(await _context.Versions.ToListAsync());

        // GET: Admin/Versions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Version version = await _context.Versions.FirstOrDefaultAsync(m => m.Id == id);

            if(version == null)
            {
                return NotFound();
            }

            return View(version);
        }

        // GET: Admin/Versions/Create
        public IActionResult Create() => View();

        // POST: Admin/Versions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Value,Count")] Version version)
        {
            if(ModelState.IsValid)
            {
                _context.Add(version);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(version);
        }

        // GET: Admin/Versions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Version version = await _context.Versions.FindAsync(id);

            if(version == null)
            {
                return NotFound();
            }

            return View(version);
        }

        // POST: Admin/Versions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Value,Count")] Version version)
        {
            if(id != version.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(version);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!VersionExists(version.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(version);
        }

        // GET: Admin/Versions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Version version = await _context.Versions.FirstOrDefaultAsync(m => m.Id == id);

            if(version == null)
            {
                return NotFound();
            }

            return View(version);
        }

        // POST: Admin/Versions/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Version version = await _context.Versions.FindAsync(id);
            _context.Versions.Remove(version);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool VersionExists(int id) => _context.Versions.Any(e => e.Id == id);
    }
}