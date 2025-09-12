using Aaru.CommonTypes.Metadata;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Mmc.Features;

public partial class List
{
    bool              _initialized;
    List<MmcFeatures> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.MmcFeatures.ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}