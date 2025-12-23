// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Find.razor.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ License ] --------------------------------------------------------------
//
//     This library is free software; you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as
//     published by the Free Software Foundation; either version 2.1 of the
//     License, or (at your option) any later version.
//
//     This library is distributed in the hope that it will be useful, but
//     WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//     Lesser General Public License for more details.
//
//     You should have received a copy of the GNU Lesser General Public
//     License along with this library; if not, see <http://www.gnu.org/licenses/>.
//
// ----------------------------------------------------------------------------
// Copyright © 2011-2026 Natalia Portillo
// ****************************************************************************/

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