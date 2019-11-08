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
    public class BlockDescriptorsController : Controller
    {
        readonly DicServerContext _context;

        public BlockDescriptorsController(DicServerContext context) => _context = context;

        // GET: Admin/BlockDescriptors
        public async Task<IActionResult> Index() => View(await _context.BlockDescriptor.ToListAsync());

        // GET: Admin/BlockDescriptors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            BlockDescriptor blockDescriptor = await _context.BlockDescriptor.FirstOrDefaultAsync(m => m.Id == id);

            if(blockDescriptor == null)
            {
                return NotFound();
            }

            return View(blockDescriptor);
        }

        // GET: Admin/BlockDescriptors/Create
        public IActionResult Create() => View();

        // POST: Admin/BlockDescriptors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Density,Blocks,BlockLength")]
                                                BlockDescriptor blockDescriptor)
        {
            if(ModelState.IsValid)
            {
                _context.Add(blockDescriptor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(blockDescriptor);
        }

        // GET: Admin/BlockDescriptors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            BlockDescriptor blockDescriptor = await _context.BlockDescriptor.FindAsync(id);

            if(blockDescriptor == null)
            {
                return NotFound();
            }

            return View(blockDescriptor);
        }

        // POST: Admin/BlockDescriptors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Density,Blocks,BlockLength")]
                                              BlockDescriptor blockDescriptor)
        {
            if(id != blockDescriptor.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(blockDescriptor);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!BlockDescriptorExists(blockDescriptor.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(blockDescriptor);
        }

        // GET: Admin/BlockDescriptors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            BlockDescriptor blockDescriptor = await _context.BlockDescriptor.FirstOrDefaultAsync(m => m.Id == id);

            if(blockDescriptor == null)
            {
                return NotFound();
            }

            return View(blockDescriptor);
        }

        // POST: Admin/BlockDescriptors/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            BlockDescriptor blockDescriptor = await _context.BlockDescriptor.FindAsync(id);
            _context.BlockDescriptor.Remove(blockDescriptor);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool BlockDescriptorExists(int id) => _context.BlockDescriptor.Any(e => e.Id == id);
    }
}