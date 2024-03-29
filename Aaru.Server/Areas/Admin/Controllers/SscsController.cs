using Aaru.CommonTypes.Metadata;
using Newtonsoft.Json;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public sealed class SscsController : Controller
{
    readonly AaruServerContext _context;

    public SscsController(AaruServerContext context) => _context = context;

    // GET: Admin/Sscs
    public async Task<IActionResult> Index() => View(await _context.Ssc.OrderBy(s => s.MinBlockLength).
                                                                    ThenBy(s => s.MaxBlockLength).
                                                                    ThenBy(s => s.BlockSizeGranularity).ToListAsync());

    // GET: Admin/Sscs/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if(id == null)
            return NotFound();

        Ssc ssc = await _context.Ssc.FirstOrDefaultAsync(m => m.Id == id);

        if(ssc == null)
            return NotFound();

        return View(ssc);
    }

    // POST: Admin/Sscs/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Ssc ssc = await _context.Ssc.FindAsync(id);
        _context.Ssc.Remove(ssc);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Consolidate()
    {
        var dups = _context.Ssc.GroupBy(x => new
        {
            x.BlockSizeGranularity,
            x.MaxBlockLength,
            x.MinBlockLength
        }).Where(x => x.Count() > 1).Select(x => new SscModel
        {
            BlockSizeGranularity = x.Key.BlockSizeGranularity,
            MaxBlockLength       = x.Key.MaxBlockLength,
            MinBlockLength       = x.Key.MinBlockLength
        }).ToList();

        return View(new SscModelForView
        {
            List = dups,
            Json = JsonConvert.SerializeObject(dups)
        });
    }

    [HttpPost]
    [ActionName("Consolidate")]
    [ValidateAntiForgeryToken]
    public IActionResult ConsolidateConfirmed(string models)
    {
        SscModel[] duplicates;

        try
        {
            duplicates = JsonConvert.DeserializeObject<SscModel[]>(models);
        }
        catch(JsonSerializationException)
        {
            return BadRequest();
        }

        if(duplicates is null)
            return BadRequest();

        foreach(SscModel duplicate in duplicates)
        {
            Ssc master = _context.Ssc.FirstOrDefault(m => m.BlockSizeGranularity == duplicate.BlockSizeGranularity &&
                                                          m.MaxBlockLength       == duplicate.MaxBlockLength       &&
                                                          m.MinBlockLength       == duplicate.MinBlockLength);

            if(master is null)
                continue;

            foreach(Ssc ssc in _context.Ssc.Where(m => m.BlockSizeGranularity == duplicate.BlockSizeGranularity &&
                                                       m.MaxBlockLength == duplicate.MaxBlockLength &&
                                                       m.MinBlockLength == duplicate.MinBlockLength).Skip(1).ToArray())
            {
                foreach(TestedSequentialMedia media in _context.TestedSequentialMedia.Where(d => d.SscId == ssc.Id))
                    media.SscId = master.Id;

                _context.Ssc.Update(ssc);
                _context.Ssc.Remove(ssc);
            }
        }

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}