using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Ata;

public partial class Details
{
    private int               _compareId;
    bool                      _initialized;
    CommonTypes.Metadata.Ata? _model;

    [Parameter]
    public int Id { get; set; }

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _model = await ctx.Ata.FirstOrDefaultAsync(m => m.Id == Id);

        _initialized = true;

        StateHasChanged();
    }

    private void GoToCompare()
    {
        if(_compareId <= 0 || !(_model?.Id > 0)) return;

        string url = $"/admin/ata/{_model.Id}/compare/{_compareId}";
        NavigationManager.NavigateTo(url);
    }
}