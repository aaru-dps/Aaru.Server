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
    public class TestedMediasController : Controller
    {
        readonly DicServerContext _context;

        public TestedMediasController(DicServerContext context) => _context = context;

        // GET: Admin/TestedMedias
        public async Task<IActionResult> Index() => View(await _context.
                                                               TestedMedia.OrderBy(m => m.Manufacturer).
                                                               ThenBy(m => m.Model).ThenBy(m => m.MediumTypeName).
                                                               ThenBy(m => m.MediaIsRecognized).
                                                               ThenBy(m => m.LongBlockSize).ThenBy(m => m.BlockSize).
                                                               ThenBy(m => m.Blocks).ToListAsync());

        // GET: Admin/TestedMedias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            TestedMedia testedMedia = await _context.TestedMedia.FirstOrDefaultAsync(m => m.Id == id);

            if(testedMedia == null)
            {
                return NotFound();
            }

            return View(testedMedia);
        }

        // GET: Admin/TestedMedias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            TestedMedia testedMedia = await _context.TestedMedia.FindAsync(id);

            if(testedMedia == null)
            {
                return NotFound();
            }

            return View(testedMedia);
        }

        // POST: Admin/TestedMedias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, [Bind("Id,Blocks,BlockSize,LongBlockSize,Manufacturer,MediumTypeName,Model")]
            TestedMedia testedMedia)
        {
            if(id != testedMedia.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(testedMedia);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!TestedMediaExists(testedMedia.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(testedMedia);
        }

        // GET: Admin/TestedMedias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            TestedMedia testedMedia = await _context.TestedMedia.FirstOrDefaultAsync(m => m.Id == id);

            if(testedMedia == null)
            {
                return NotFound();
            }

            return View(testedMedia);
        }

        // POST: Admin/TestedMedias/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            TestedMedia testedMedia = await _context.TestedMedia.FindAsync(id);
            _context.TestedMedia.Remove(testedMedia);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool TestedMediaExists(int id) => _context.TestedMedia.Any(e => e.Id == id);
    }
}