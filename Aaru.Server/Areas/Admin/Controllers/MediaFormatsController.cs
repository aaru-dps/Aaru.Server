namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class MediaFormatsController : Controller
{
    readonly AaruServerContext _context;

    public MediaFormatsController(AaruServerContext context) => _context = context;

    // GET: Admin/MediaFormats
    public async Task<IActionResult> Index() => View(await _context.MediaFormats.OrderBy(mf => mf.Name).ToListAsync());

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
}