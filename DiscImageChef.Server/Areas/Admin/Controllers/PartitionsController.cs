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
    public class PartitionsController : Controller
    {
        readonly DicServerContext _context;

        public PartitionsController(DicServerContext context) => _context = context;

        // GET: Admin/Partitions
        public async Task<IActionResult> Index() => View(await _context.Partitions.ToListAsync());

        // GET: Admin/Partitions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Partition partition = await _context.Partitions.FirstOrDefaultAsync(m => m.Id == id);

            if(partition == null)
            {
                return NotFound();
            }

            return View(partition);
        }

        // GET: Admin/Partitions/Create
        public IActionResult Create() => View();

        // POST: Admin/Partitions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Count")] Partition partition)
        {
            if(ModelState.IsValid)
            {
                _context.Add(partition);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(partition);
        }

        // GET: Admin/Partitions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Partition partition = await _context.Partitions.FindAsync(id);

            if(partition == null)
            {
                return NotFound();
            }

            return View(partition);
        }

        // POST: Admin/Partitions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Count")] Partition partition)
        {
            if(id != partition.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(partition);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!PartitionExists(partition.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(partition);
        }

        // GET: Admin/Partitions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Partition partition = await _context.Partitions.FirstOrDefaultAsync(m => m.Id == id);

            if(partition == null)
            {
                return NotFound();
            }

            return View(partition);
        }

        // POST: Admin/Partitions/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Partition partition = await _context.Partitions.FindAsync(id);
            _context.Partitions.Remove(partition);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool PartitionExists(int id) => _context.Partitions.Any(e => e.Id == id);
    }
}