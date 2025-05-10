using Aaru.Server.Database.Models;
using Blazorise.Charts;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Pages.Statistics;

public partial class Filesystems
{
    PieChart<long>?  _filesystemsChart;
    List<long>       _filesystemsCounts = [];
    string[]         _filesystemsLabels = [];
    bool             _isAlreadyInitialized;
    List<Filesystem> FilesystemsList { get; set; } = [];

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        FilesystemsList = await ctx.Filesystems.OrderBy(static filesystem => filesystem.Name).ToListAsync();

        await base.OnInitializedAsync();
    }

    /// <inheritdoc />
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_isAlreadyInitialized) return;

        _isAlreadyInitialized = true;
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _filesystemsLabels = await ctx.Filesystems.OrderByDescending(static o => o.Count)
                                      .Take(10)
                                      .Select(static v => v.Name)
                                      .ToArrayAsync();

        _filesystemsCounts = await ctx.Filesystems.OrderByDescending(static o => o.Count)
                                      .Take(10)
                                      .Select(static x => x.Count)
                                      .ToListAsync();

        if(_filesystemsLabels.Length >= 10)
        {
            _filesystemsLabels[9] = "Other";

            _filesystemsCounts[9] = ctx.Filesystems.Sum(static o => o.Count) - _filesystemsCounts.Take(9).Sum();
        }

#pragma warning disable CS8604 // Possible null reference argument.
        await Common.HandleRedrawAsync(_filesystemsChart, _filesystemsLabels, GetFilesystemsChartDataset);
#pragma warning restore CS8604 // Possible null reference argument.
    }

    PieChartDataset<long> GetFilesystemsChartDataset() => new()
    {
        Label           = $"Top {_filesystemsLabels.Length} filesystems found",
        Data            = _filesystemsCounts,
        BackgroundColor = Common.BackgroundColors,
        BorderColor     = Common.BorderColors,
        BorderWidth     = 1
    };
}