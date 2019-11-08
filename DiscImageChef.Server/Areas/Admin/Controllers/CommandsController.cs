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
    public class CommandsController : Controller
    {
        readonly DicServerContext _context;

        public CommandsController(DicServerContext context) => _context = context;

        // GET: Admin/Commands
        public async Task<IActionResult> Index() => View(await _context.Commands.ToListAsync());

        // GET: Admin/Commands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Command command = await _context.Commands.FirstOrDefaultAsync(m => m.Id == id);

            if(command == null)
            {
                return NotFound();
            }

            return View(command);
        }

        // GET: Admin/Commands/Create
        public IActionResult Create() => View();

        // POST: Admin/Commands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Count")] Command command)
        {
            if(ModelState.IsValid)
            {
                _context.Add(command);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        // GET: Admin/Commands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Command command = await _context.Commands.FindAsync(id);

            if(command == null)
            {
                return NotFound();
            }

            return View(command);
        }

        // POST: Admin/Commands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Count")] Command command)
        {
            if(id != command.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _context.Update(command);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!CommandExists(command.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        // GET: Admin/Commands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            Command command = await _context.Commands.FirstOrDefaultAsync(m => m.Id == id);

            if(command == null)
            {
                return NotFound();
            }

            return View(command);
        }

        // POST: Admin/Commands/Delete/5
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Command command = await _context.Commands.FindAsync(id);
            _context.Commands.Remove(command);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        bool CommandExists(int id) => _context.Commands.Any(e => e.Id == id);
    }
}