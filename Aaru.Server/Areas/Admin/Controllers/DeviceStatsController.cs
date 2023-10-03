namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class DeviceStatsController : Controller
{
    readonly AaruServerContext _context;

    public DeviceStatsController(AaruServerContext context) => _context = context;

    // GET: Admin/DeviceStats
    public async Task<IActionResult> Index() => View(await _context.DeviceStats.OrderBy(d => d.Manufacturer).
                                                                    ThenBy(d => d.Model).ThenBy(d => d.Bus).
                                                                    ToListAsync());

    // GET: Admin/DeviceStats/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if(id == null)
            return NotFound();

        DeviceStat deviceStat = await _context.DeviceStats.FindAsync(id);

        if(deviceStat == null)
            return NotFound();

        return View(deviceStat);
    }

    // POST: Admin/DeviceStats/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Manufacturer,Model,Revision,Bus")] DeviceStat changedModel)
    {
        if(id != changedModel.Id)
            return NotFound();

        if(!ModelState.IsValid)
            return View(changedModel);

        DeviceStat model = await _context.DeviceStats.FirstOrDefaultAsync(m => m.Id == id);

        if(model is null)
            return NotFound();

        model.Manufacturer = changedModel.Manufacturer;
        model.Model        = changedModel.Model;
        model.Revision     = changedModel.Revision;
        model.Bus          = changedModel.Bus;

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

    // GET: Admin/DeviceStats/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if(id == null)
            return NotFound();

        DeviceStat deviceStat = await _context.DeviceStats.FirstOrDefaultAsync(m => m.Id == id);

        if(deviceStat == null)
            return NotFound();

        return View(deviceStat);
    }

    // POST: Admin/DeviceStats/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        DeviceStat deviceStat = await _context.DeviceStats.FindAsync(id);
        _context.DeviceStats.Remove(deviceStat);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}