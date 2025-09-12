using Aaru.CommonTypes.Metadata;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.MmcSds;

public partial class Details
{
    bool   _initialized;
    MmcSd? _model;
    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _model = await ctx.MmcSd.FirstOrDefaultAsync(m => m.Id == Id);

        _initialized = true;

        StateHasChanged();
    }
}