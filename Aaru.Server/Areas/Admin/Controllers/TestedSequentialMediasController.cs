using Aaru.CommonTypes.Metadata;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class TestedSequentialMediasController : Controller
{
    readonly AaruServerContext _context;

    public TestedSequentialMediasController(AaruServerContext context) => _context = context;

    // GET: Admin/TestedSequentialMedias
    public async Task<IActionResult> Index() => View(await _context.TestedSequentialMedia.OrderBy(m => m.Manufacturer).
                                                                    ThenBy(m => m.Model).ThenBy(m => m.MediumTypeName).
                                                                    ToListAsync());

    // GET: Admin/TestedSequentialMedias/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if(id == null)
            return NotFound();

        TestedSequentialMedia testedSequentialMedia = await _context.TestedSequentialMedia.FindAsync(id);

        if(testedSequentialMedia == null)
            return NotFound();

        return View(testedSequentialMedia);
    }

    // POST: Admin/TestedSequentialMedias/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, [Bind("Id,Manufacturer,MediumTypeName,Model")] TestedSequentialMedia changedModel)
    {
        if(id != changedModel.Id)
            return NotFound();

        if(!ModelState.IsValid)
            return View(changedModel);

        TestedSequentialMedia model = await _context.TestedSequentialMedia.FirstOrDefaultAsync(m => m.Id == id);

        if(model is null)
            return NotFound();

        model.Manufacturer   = changedModel.Manufacturer;
        model.MediumTypeName = changedModel.MediumTypeName;
        model.Model          = changedModel.Model;

        try
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateConcurrencyException)
        {
            ModelState.AddModelError("Concurrency", "Concurrency error, please report to the administrator.");
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/TestedSequentialMedias/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if(id == null)
            return NotFound();

        TestedSequentialMedia testedSequentialMedia =
            await _context.TestedSequentialMedia.FirstOrDefaultAsync(m => m.Id == id);

        if(testedSequentialMedia == null)
            return NotFound();

        return View(testedSequentialMedia);
    }

    // POST: Admin/TestedSequentialMedias/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        TestedSequentialMedia testedSequentialMedia = await _context.TestedSequentialMedia.FindAsync(id);
        _context.TestedSequentialMedia.Remove(testedSequentialMedia);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}