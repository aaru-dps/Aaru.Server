using System.ComponentModel.DataAnnotations;
using Aaru.CommonTypes.Enums;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Devices;

public partial class Edit
{
    DbContext db;

    DeviceEditViewModel device;
    bool                isLoaded;
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        db = DbContextFactory.CreateDbContext();
        Device? entity = await db.Devices.FindAsync(Id);

        if(entity != null)
        {
            device = new DeviceEditViewModel
            {
                Id                         = entity.Id,
                Manufacturer               = entity.Manufacturer,
                Model                      = entity.Model,
                Revision                   = entity.Revision,
                CompactFlash               = entity.CompactFlash,
                OptimalMultipleSectorsRead = entity.OptimalMultipleSectorsRead,
                CanReadGdRomUsingSwapDisc  = entity.CanReadGdRomUsingSwapDisc ?? false,
                Type                       = entity.Type
            };
        }

        isLoaded = true;
    }

    protected async Task HandleValidSubmit()
    {
        Device? entity = await db.Devices.FindAsync(Id);

        if(entity != null)
        {
            entity.Manufacturer               = device.Manufacturer;
            entity.Model                      = device.Model;
            entity.Revision                   = device.Revision;
            entity.CompactFlash               = device.CompactFlash;
            entity.OptimalMultipleSectorsRead = device.OptimalMultipleSectorsRead;
            entity.CanReadGdRomUsingSwapDisc  = device.CanReadGdRomUsingSwapDisc;
            entity.Type                       = device.Type;
            entity.ModifiedWhen               = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }

        Navigation.NavigateTo("/admin/devices");
    }

    protected void GoBack()
    {
        Navigation.NavigateTo("/admin/devices");
    }

    public class DeviceEditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Model { get;        set; }
        public string Revision     { get; set; }
        public bool   CompactFlash { get; set; }
        [Range(0, int.MaxValue)]
        public int OptimalMultipleSectorsRead { get; set; }
        public bool CanReadGdRomUsingSwapDisc { get; set; }
        [Required]
        public DeviceType Type { get; set; }
    }
}