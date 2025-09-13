using Aaru.CommonTypes.Metadata;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Gdrom;

public partial class Details
{
    bool                       _initialized;
    GdRomSwapDiscCapabilities? _model;
    [Parameter]
    public int Id { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _model = await ctx.GdRomSwapDiscCapabilities.FirstOrDefaultAsync(m => m.Id == Id);
    }
}