using System.Threading.Tasks;
using Aaru.CommonTypes.Metadata;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class PcmciasController : Controller
{
    readonly AaruServerContext _context;

    public PcmciasController(AaruServerContext context) => _context = context;

    // GET: Admin/Pcmcias
    public async Task<IActionResult> Index() => View(await _context.Pcmcia.ToListAsync());

    // GET: Admin/Pcmcias/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if(id == null)
        {
            return NotFound();
        }

        Pcmcia pcmcia = await _context.Pcmcia.FirstOrDefaultAsync(m => m.Id == id);

        if(pcmcia == null)
        {
            return NotFound();
        }

        return View(pcmcia);
    }

    // POST: Admin/Pcmcias/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Pcmcia pcmcia = await _context.Pcmcia.FindAsync(id);
        _context.Pcmcia.Remove(pcmcia);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}