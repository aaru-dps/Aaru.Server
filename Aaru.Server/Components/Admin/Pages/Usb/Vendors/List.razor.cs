using Aaru.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Usb.Vendors;

public partial class List
{
    bool _initialized;

    List<UsbVendor> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.UsbVendors.OrderBy(static v => v.Vendor).ThenBy(static v => v.VendorId).ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}