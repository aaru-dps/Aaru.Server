using System.ComponentModel.DataAnnotations;
using Aaru.CommonTypes.Enums;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Reports;

public partial class Edit : ComponentBase
{
    DbContext db;
    bool      isLoaded;

    UploadedReportEditViewModel report;
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        db = await DbContextFactory.CreateDbContextAsync();
        UploadedReport? entity = await db.Reports.FindAsync(Id);

        if(entity != null)
        {
            report = new UploadedReportEditViewModel
            {
                Id           = entity.Id,
                Manufacturer = entity.Manufacturer,
                Model        = entity.Model,
                Revision     = entity.Revision,
                CompactFlash = entity.CompactFlash,
                Type         = entity.Type
            };
        }

        isLoaded = true;
    }

    protected async Task HandleValidSubmit()
    {
        UploadedReport? entity = await db.Reports.FindAsync(Id);

        if(entity != null)
        {
            entity.Manufacturer = report.Manufacturer;
            entity.Model        = report.Model;
            entity.Revision     = report.Revision;
            entity.CompactFlash = report.CompactFlash;
            entity.Type         = report.Type;
            await db.SaveChangesAsync();
        }

        Navigation.NavigateTo("/admin/reports");
    }

    protected void GoBack()
    {
        Navigation.NavigateTo("/admin/reports");
    }

    public class UploadedReportEditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Model { get;        set; }
        public string Revision     { get; set; }
        public bool   CompactFlash { get; set; }
        [Required]
        public DeviceType Type { get; set; }
    }
}