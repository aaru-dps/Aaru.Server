using System.Linq;
using System.Threading.Tasks;
using DiscImageChef.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscImageChef.Server.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize]
    public class MediasController : Controller
    {
        readonly DicServerContext _context;

        public MediasController(DicServerContext context) => _context = context;

        // GET: Admin/Medias
        public async Task<IActionResult> Index() => View(await _context.Medias.ToListAsync());

        // GET: Admin/Medias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Media media = await _context.Medias.FirstOrDefaultAsync(m => m.Id == id);

            if(media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        // GET: Admin/Medias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Media media = await _context.Medias.FindAsync(id);

            if(media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        // POST: Admin/Medias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Real,Count")] Media media)
        {
            if(id != media.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(media);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!MediaExists(media.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(media);
        }

        // GET: Admin/Medias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Media media = await _context.Medias.FirstOrDefaultAsync(m => m.Id == id);

            if(media == null)
            {
                return NotFound();
            }

            return View(media);
        }

        // POST: Admin/Medias/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Media media = await _context.Medias.FindAsync(id);
            _context.Medias.Remove(media);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool MediaExists(int id) => _context.Medias.Any(e => e.Id == id);
    }
}