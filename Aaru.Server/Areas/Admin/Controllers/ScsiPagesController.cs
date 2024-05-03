using Aaru.CommonTypes.Metadata;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class ScsiPagesController : Controller
{
    readonly DbContext _context;

    public ScsiPagesController(DbContext context) => _context = context;

    // GET: Admin/ScsiPages
    public async Task<IActionResult> Index() => View(await _context.ScsiPage.ToListAsync());

    // GET: Admin/ScsiPages/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if(id == null) return NotFound();

        ScsiPage scsiPage = await _context.ScsiPage.FirstOrDefaultAsync(m => m.Id == id);

        if(scsiPage == null) return NotFound();

        return View(scsiPage);
    }

    // POST: Admin/ScsiPages/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        ScsiPage scsiPage = await _context.ScsiPage.FindAsync(id);
        _context.ScsiPage.Remove(scsiPage);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}