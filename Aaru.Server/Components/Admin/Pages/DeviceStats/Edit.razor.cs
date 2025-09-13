using System.ComponentModel.DataAnnotations;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.DeviceStats;

public partial class Edit : ComponentBase
{
    DbContext db;

    DeviceStatEditViewModel deviceStat;
    bool                    isLoaded;
    [Parameter]
    public int Id { get; set; }


    [Inject]
    public NavigationManager Navigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        db = await DbContextFactory.CreateDbContextAsync();
        DeviceStat? entity = await db.DeviceStats.FindAsync(Id);

        if(entity != null)
        {
            deviceStat = new DeviceStatEditViewModel
            {
                Id           = entity.Id,
                Manufacturer = entity.Manufacturer,
                Model        = entity.Model,
                Revision     = entity.Revision,
                Bus          = entity.Bus
            };
        }

        isLoaded = true;
    }

    protected async Task HandleValidSubmit()
    {
        DeviceStat? entity = await db.DeviceStats.FindAsync(Id);

        if(entity != null)
        {
            entity.Manufacturer = deviceStat.Manufacturer;
            entity.Model        = deviceStat.Model;
            entity.Revision     = deviceStat.Revision;
            entity.Bus          = deviceStat.Bus;
            await db.SaveChangesAsync();
        }

        Navigation.NavigateTo("/admin/device-stats");
    }

    protected void GoBack()
    {
        Navigation.NavigateTo("/admin/device-stats");
    }

    public class DeviceStatEditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string? Manufacturer { get; set; }
        [Required]
        public string? Model { get;    set; }
        public string? Revision { get; set; }
        [Required]
        public string Bus { get; set; }
    }
}