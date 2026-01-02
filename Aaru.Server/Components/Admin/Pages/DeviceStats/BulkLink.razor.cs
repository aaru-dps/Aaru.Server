// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : BulkLink.razor.cs
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

using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.DeviceStats;

public partial class BulkLink
{
    private readonly List<MatchedStatReportPair> _matchedPairs = new();
    private          bool                        _initialized;
    private          bool                        _isProcessing;
    private          bool                        _showConfirmModal;
    private          bool                        _showSuccessMessage;
    private          int                         _successCount;
    private          List<DeviceStat>            _unlinkedStats = new();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        // Get all device statistics that don't have a linked report
        _unlinkedStats = await ctx.DeviceStats.Where(s => s.Report == null)
                                  .OrderBy(s => s.Manufacturer)
                                  .ThenBy(s => s.Model)
                                  .ThenBy(s => s.Revision)
                                  .ThenBy(s => s.Bus)
                                  .ToListAsync();

        // For each unlinked stat, find matching device reports
        foreach(DeviceStat stat in _unlinkedStats)
        {
            // Search for device reports that match manufacturer, model, and firmware revision
            List<Device> matchingDevices = await ctx.Devices
                                                    .Where(d => d.Manufacturer == stat.Manufacturer &&
                                                                d.Model        == stat.Model        &&
                                                                d.Revision     == stat.Revision)
                                                    .OrderBy(d => d.Id)
                                                    .ToListAsync();

            // If we found exactly one match, add it to our list
            if(matchingDevices.Count == 1)
                _matchedPairs.Add(new MatchedStatReportPair(stat, matchingDevices[0]));
            else if(matchingDevices.Count > 1)
            {
                // If multiple matches, add only the first one but mark as needs review
                _matchedPairs.Add(new MatchedStatReportPair(stat, matchingDevices[0]));
            }
        }

        _initialized = true;
        StateHasChanged();
    }

    private void ToggleSelectAll(bool selected)
    {
        foreach(MatchedStatReportPair pair in _matchedPairs) pair.IsSelected = selected;

        StateHasChanged();
    }

    private void ShowConfirmModal()
    {
        if(!_matchedPairs.Any(p => p.IsSelected)) return;

        _showConfirmModal = true;
        StateHasChanged();
    }

    private async Task ConfirmLinkAsync()
    {
        _isProcessing = true;
        StateHasChanged();

        try
        {
            await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

            var selectedPairs = _matchedPairs.Where(p => p.IsSelected).ToList();

            foreach(MatchedStatReportPair pair in selectedPairs)
            {
                // Reload the stat from the database to ensure we have the latest data
                DeviceStat? stat = await ctx.DeviceStats.FirstOrDefaultAsync(s => s.Id == pair.Stat.Id);

                if(stat != null)
                {
                    stat.Report = pair.Report;
                    ctx.Update(stat);
                }
            }

            await ctx.SaveChangesAsync();

            _successCount       = selectedPairs.Count;
            _showConfirmModal   = false;
            _showSuccessMessage = true;

            // Hide the success message after 3 seconds and redirect
            await Task.Delay(3000);
            NavigationManager.NavigateTo("/admin/device-stats");
        }
        finally
        {
            _isProcessing = false;
            StateHasChanged();
        }
    }

    private sealed class MatchedStatReportPair
    {
        public MatchedStatReportPair(DeviceStat stat, Device report)
        {
            Stat   = stat;
            Report = report;
        }

        public DeviceStat Stat       { get; }
        public Device     Report     { get; }
        public bool       IsSelected { get; set; }
    }
}