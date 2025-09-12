using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Scsi.Cdrom;

public partial class Details
{
    bool                      _initialized;
    CommonTypes.Metadata.Mmc? _model;
    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _model = await ctx.Mmc.FirstOrDefaultAsync(m => m.Id == Id);

        _initialized = true;

        StateHasChanged();
    }
}