using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using OperatingSystem = Aaru.Server.Database.Models.OperatingSystem;

namespace Aaru.Server.Components.Admin.Pages.OperatingSystems;

public partial class View
{
    bool                  _initialized;
    List<OperatingSystem> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.OperatingSystems.OrderBy(static o => o.Name).ThenBy(static o => o.Version).ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}