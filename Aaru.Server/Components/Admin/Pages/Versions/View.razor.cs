using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using Version = Aaru.Server.Database.Models.Version;

namespace Aaru.Server.Components.Admin.Pages.Versions;

public partial class View
{
    bool          _initialized;
    List<Version> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Versions.OrderBy(static v => v.Name).ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}