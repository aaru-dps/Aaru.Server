using Aaru.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Commands;

public partial class View
{
    bool          _initialized;
    List<Command> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Commands.OrderBy(static c => c.Name).ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}