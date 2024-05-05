using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.New.Components.Pages.Report;

public partial class View
{
    bool   _initialized;
    bool   _notFound;
    string _pageTitle { get; set; } = "Aaru Device Report";
    [CascadingParameter]
    HttpContext HttpContext { get; set; } = default!;

    [Parameter]
    public int Id { get; set; }

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        if(Id <= 0)
        {
            _notFound                       = true;
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            return;
        }

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        Device? report = await ctx.Devices.FirstOrDefaultAsync(d => d.Id == Id);

        if(report is null)
        {
            _notFound                       = true;
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            return;
        }

        _pageTitle = $"Aaru Device Report for {report.Manufacturer} {report.Model} {report.Revision}";

        _initialized = true;

        StateHasChanged();
    }
}