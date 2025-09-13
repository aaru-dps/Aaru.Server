using System.Collections.Specialized;
using System.Web;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Devices;

public partial class Find
{
    FindReportModel _found;
    bool            _initialized;
    [Parameter]
    public int Id { get; set; }

    [Inject]
    NavigationManager Navigation { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        // Get query parameters
        Uri                 uri   = Navigation.ToAbsoluteUri(Navigation.Uri);
        NameValueCollection query = HttpUtility.ParseQueryString(uri.Query);
        string              model = query["model"] ?? "";

        _found = new FindReportModel
        {
            Id           = Id,
            Manufacturer = query["manufacturer"],
            Model        = model,
            Revision     = query["revision"],
            Bus          = query["bus"],
#pragma warning disable RCS1155
            LikeDevices = await ctx.Devices.Where(r => r.Model.ToLower().Contains(model.ToLower())).ToListAsync()
#pragma warning restore RCS1155
        };

        _initialized = true;
        StateHasChanged();
    }

    async Task LinkReports(int deviceId, int statsId)
    {
        await using DbContext ctx    = await DbContextFactory.CreateDbContextAsync();
        Device?               device = await ctx.Devices.FirstOrDefaultAsync(m => m.Id     == deviceId);
        DeviceStat?           stat   = await ctx.DeviceStats.FirstOrDefaultAsync(m => m.Id == statsId);

        if(stat != null)
        {
            stat.Report = device;
            ctx.Update(stat);
        }

        await ctx.SaveChangesAsync();
        Id = deviceId;

        Navigation.NavigateTo($"/admin/devices/{deviceId}");
    }
}