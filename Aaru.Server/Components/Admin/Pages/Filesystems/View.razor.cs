using Aaru.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Filesystems;

public partial class View
{
    bool             _initialized;
    List<Filesystem> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Filesystems.OrderBy(static f => f.Name).ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}