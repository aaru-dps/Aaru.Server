using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aaru.CommonTypes.Metadata;
using Aaru.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Aaru.Server.Areas.Admin.Controllers;

[Area("Admin"), Authorize]
public sealed class ChsController : Controller
{
    readonly AaruServerContext _context;

    public ChsController(AaruServerContext context) => _context = context;

    // GET: Admin/Chs
    public async Task<IActionResult> Index() => View(await _context.Chs.OrderBy(c => c.Cylinders).
                                                                    ThenBy(c => c.Heads).ThenBy(c => c.Sectors).
                                                                    ToListAsync());

    public IActionResult Consolidate()
    {
        List<ChsModel> dups = _context.Chs.GroupBy(x => new
        {
            x.Cylinders,
            x.Heads,
            x.Sectors
        }).Where(x => x.Count() > 1).Select(x => new ChsModel
        {
            Cylinders = x.Key.Cylinders,
            Heads     = x.Key.Heads,
            Sectors   = x.Key.Sectors
        }).ToList();

        return View(new ChsModelForView
        {
            List = dups,
            Json = JsonConvert.SerializeObject(dups)
        });
    }

    [HttpPost, ActionName("Consolidate"), ValidateAntiForgeryToken]
    public IActionResult ConsolidateConfirmed(string models)
    {
        ChsModel[] duplicates;

        try
        {
            duplicates = JsonConvert.DeserializeObject<ChsModel[]>(models);
        }
        catch(JsonSerializationException)
        {
            return BadRequest();
        }

        if(duplicates is null)
            return BadRequest();

        foreach(ChsModel duplicate in duplicates)
        {
            Chs master = _context.Chs.FirstOrDefault(m => m.Cylinders == duplicate.Cylinders &&
                                                          m.Heads     == duplicate.Heads     &&
                                                          m.Sectors   == duplicate.Sectors);

            if(master is null)
                continue;

            foreach(Chs chs in _context.Chs.Where(m => m.Cylinders == duplicate.Cylinders &&
                                                       m.Heads     == duplicate.Heads     &&
                                                       m.Sectors   == duplicate.Sectors).Skip(1).ToArray())
            {
                foreach(TestedMedia media in _context.TestedMedia.Where(d => d.CHS.Id == chs.Id))
                {
                    media.CHS = master;
                }

                foreach(TestedMedia media in _context.TestedMedia.Where(d => d.CurrentCHS.Id == chs.Id))
                {
                    media.CurrentCHS = master;
                }

                _context.Chs.Remove(chs);
            }
        }

        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}