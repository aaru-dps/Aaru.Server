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
    public class MediaFormatsController : Controller
    {
        readonly DicServerContext _context;

        public MediaFormatsController(DicServerContext context) => _context = context;

        // GET: Admin/MediaFormats
        public async Task<IActionResult> Index() => View(await _context.MediaFormats.ToListAsync());

        // GET: Admin/MediaFormats/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            MediaFormat mediaFormat = await _context.MediaFormats.FirstOrDefaultAsync(m => m.Id == id);

            if(mediaFormat == null)
            {
                return NotFound();
            }

            return View(mediaFormat);
        }

        // GET: Admin/MediaFormats/Create
        public IActionResult Create() => View();

        // POST: Admin/MediaFormats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Count")] MediaFormat mediaFormat)
        {
            if(ModelState.IsValid)
            {
                _context.Add(mediaFormat);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(mediaFormat);
        }

        // GET: Admin/MediaFormats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            MediaFormat mediaFormat = await _context.MediaFormats.FindAsync(id);

            if(mediaFormat == null)
            {
                return NotFound();
            }

            return View(mediaFormat);
        }

        // POST: Admin/MediaFormats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Count")] MediaFormat mediaFormat)
        {
            if(id != mediaFormat.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(mediaFormat);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!MediaFormatExists(mediaFormat.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(mediaFormat);
        }

        // GET: Admin/MediaFormats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            MediaFormat mediaFormat = await _context.MediaFormats.FirstOrDefaultAsync(m => m.Id == id);

            if(mediaFormat == null)
            {
                return NotFound();
            }

            return View(mediaFormat);
        }

        // POST: Admin/MediaFormats/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            MediaFormat mediaFormat = await _context.MediaFormats.FindAsync(id);
            _context.MediaFormats.Remove(mediaFormat);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool MediaFormatExists(int id) => _context.MediaFormats.Any(e => e.Id == id);
    }
}