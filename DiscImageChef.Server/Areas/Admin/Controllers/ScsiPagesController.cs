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
    public class ScsiPagesController : Controller
    {
        readonly DicServerContext _context;

        public ScsiPagesController(DicServerContext context) => _context = context;

        // GET: Admin/ScsiPages
        public async Task<IActionResult> Index() => View(await _context.ScsiPage.ToListAsync());

        // GET: Admin/ScsiPages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ScsiPage scsiPage = await _context.ScsiPage.FirstOrDefaultAsync(m => m.Id == id);

            if(scsiPage == null)
            {
                return NotFound();
            }

            return View(scsiPage);
        }

        // GET: Admin/ScsiPages/Create
        public IActionResult Create() => View();

        // POST: Admin/ScsiPages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,page,subpage,value")] ScsiPage scsiPage)
        {
            if(ModelState.IsValid)
            {
                _context.Add(scsiPage);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(scsiPage);
        }

        // GET: Admin/ScsiPages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ScsiPage scsiPage = await _context.ScsiPage.FindAsync(id);

            if(scsiPage == null)
            {
                return NotFound();
            }

            return View(scsiPage);
        }

        // POST: Admin/ScsiPages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,page,subpage,value")] ScsiPage scsiPage)
        {
            if(id != scsiPage.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(scsiPage);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!ScsiPageExists(scsiPage.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(scsiPage);
        }

        // GET: Admin/ScsiPages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            ScsiPage scsiPage = await _context.ScsiPage.FirstOrDefaultAsync(m => m.Id == id);

            if(scsiPage == null)
            {
                return NotFound();
            }

            return View(scsiPage);
        }

        // POST: Admin/ScsiPages/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ScsiPage scsiPage = await _context.ScsiPage.FindAsync(id);
            _context.ScsiPage.Remove(scsiPage);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool ScsiPageExists(int id) => _context.ScsiPage.Any(e => e.Id == id);
    }
}