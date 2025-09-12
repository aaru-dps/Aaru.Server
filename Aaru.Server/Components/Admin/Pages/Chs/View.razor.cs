using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Chs;

public partial class View
{
    private Modal?                 _consolidateModal;
    List<ChsModel>                 _duplicates;
    bool                           _initialized;
    List<CommonTypes.Metadata.Chs> _items;


    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Chs.OrderBy(static c => c.Cylinders)
                          .ThenBy(static c => c.Heads)
                          .ThenBy(static c => c.Sectors)
                          .ToListAsync();

        _duplicates = await ctx.Chs.GroupBy(static x => new
                                {
                                    x.Cylinders,
                                    x.Heads,
                                    x.Sectors
                                })
                               .Where(static x => x.Count() > 1)
                               .Select(static x => new ChsModel
                                {
                                    Cylinders = x.Key.Cylinders,
                                    Heads     = x.Key.Heads,
                                    Sectors   = x.Key.Sectors
                                })
                               .ToListAsync();

        _initialized = true;

        StateHasChanged();
    }

    Task ConsolidateDuplicatesAsync() => _consolidateModal?.ShowAsync();

    Task HideConsolidateModalAsync() => _consolidateModal?.HideAsync();

    async Task ConfirmConsolidateAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        foreach(ChsModel duplicate in _duplicates)
        {
            CommonTypes.Metadata.Chs? master = ctx.Chs.FirstOrDefault(m => m.Cylinders == duplicate.Cylinders &&
                                                                           m.Heads     == duplicate.Heads     &&
                                                                           m.Sectors   == duplicate.Sectors);

            if(master is null) continue;

            foreach(CommonTypes.Metadata.Chs chs in await ctx.Chs.Where(m => m.Cylinders == duplicate.Cylinders &&
                                                                             m.Heads     == duplicate.Heads     &&
                                                                             m.Sectors   == duplicate.Sectors)
                                                             .Skip(1)
                                                             .ToArrayAsync())
            {
                foreach(CommonTypes.Metadata.TestedMedia media in ctx.TestedMedia.Where(d => d.CHS.Id == chs.Id))
                    media.CHS = master;

                foreach(CommonTypes.Metadata.TestedMedia media in ctx.TestedMedia.Where(d => d.CurrentCHS.Id == chs.Id))
                    media.CurrentCHS = master;

                ctx.Chs.Remove(chs);
            }
        }

        await ctx.SaveChangesAsync();

        _items = await ctx.Chs.OrderBy(static c => c.Cylinders)
                          .ThenBy(static c => c.Heads)
                          .ThenBy(static c => c.Sectors)
                          .ToListAsync();

        _duplicates = await ctx.Chs.GroupBy(static x => new
                                {
                                    x.Cylinders,
                                    x.Heads,
                                    x.Sectors
                                })
                               .Where(static x => x.Count() > 1)
                               .Select(static x => new ChsModel
                                {
                                    Cylinders = x.Key.Cylinders,
                                    Heads     = x.Key.Heads,
                                    Sectors   = x.Key.Sectors
                                })
                               .ToListAsync();

        StateHasChanged();
    }
}