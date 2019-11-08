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

        // GET: Admin/Sscs/Create
        public IActionResult Create() => View();

        // POST: Admin/Sscs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BlockSizeGranularity,MaxBlockLength,MinBlockLength")]
                                                Ssc ssc)
        {
            if(ModelState.IsValid)
            {
                _context.Add(ssc);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(ssc);
        }

        // GET: Admin/Sscs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Ssc ssc = await _context.Ssc.FindAsync(id);

            if(ssc == null)
            {
                return NotFound();
            }

            return View(ssc);
        }

        // POST: Admin/Sscs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlockSizeGranularity,MaxBlockLength,MinBlockLength")]
                                              Ssc ssc)
        {
            if(id != ssc.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(ssc);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!SscExists(ssc.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
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

        bool SscExists(int id) => _context.Ssc.Any(e => e.Id == id);
    }
}