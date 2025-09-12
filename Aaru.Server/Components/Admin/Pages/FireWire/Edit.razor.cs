using Microsoft.AspNetCore.Components;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.FireWire;

public partial class Edit : ComponentBase
{
    DbContext _db;

    CommonTypes.Metadata.FireWire _fireWire = new();
    bool                          _isLoaded;
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _db = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.FireWire? entity = await _db.FireWire.FindAsync(Id);
        if(entity != null) _fireWire          = entity;
        _isLoaded = true;
    }

    async Task HandleValidSubmit()
    {
        _db.Update(_fireWire);
        await _db.SaveChangesAsync();
        Navigation.NavigateTo("/admin/firewire");
    }

    void GoBack()
    {
        Navigation.NavigateTo("/admin/firewire");
    }
}