using Aaru.CommonTypes.Metadata;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.BlockDescriptors;

public partial class View
{
    bool                  _initialized;
    List<BlockDescriptor> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.BlockDescriptor.OrderBy(b => b.BlockLength)
                          .ThenBy(b => b.Blocks)
                          .ThenBy(b => b.Density)
                          .ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}