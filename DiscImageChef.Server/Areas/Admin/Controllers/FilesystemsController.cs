using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FilesystemsController : Controller
    {
        readonly DicServerContext _context;

        public FilesystemsController(DicServerContext context) => _context = context;

        // GET: Admin/Filesystems
        public async Task<IActionResult> Index() => View(await _context.Filesystems.ToListAsync());

        // GET: Admin/Filesystems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Filesystem filesystem = await _context.Filesystems.FirstOrDefaultAsync(m => m.Id == id);

            if(filesystem == null)
            {
                return NotFound();
            }

            return View(filesystem);
        }

        // GET: Admin/Filesystems/Create
        public IActionResult Create() => View();

        // POST: Admin/Filesystems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Count")] Filesystem filesystem)
        {
            if(ModelState.IsValid)
            {
                _context.Add(filesystem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(filesystem);
        }

        // GET: Admin/Filesystems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Filesystem filesystem = await _context.Filesystems.FindAsync(id);

            if(filesystem == null)
            {
                return NotFound();
            }

            return View(filesystem);
        }

        // POST: Admin/Filesystems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Count")] Filesystem filesystem)
        {
            if(id != filesystem.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(filesystem);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!FilesystemExists(filesystem.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(filesystem);
        }

        // GET: Admin/Filesystems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Filesystem filesystem = await _context.Filesystems.FirstOrDefaultAsync(m => m.Id == id);

            if(filesystem == null)
            {
                return NotFound();
            }

            return View(filesystem);
        }

        // POST: Admin/Filesystems/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Filesystem filesystem = await _context.Filesystems.FindAsync(id);
            _context.Filesystems.Remove(filesystem);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool FilesystemExists(int id) => _context.Filesystems.Any(e => e.Id == id);
    }
}