using Aaru.Server.Database.Models;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Usb.Products;

public partial class List
{
    bool _initialized;

    List<UsbProductModel> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.UsbProducts.Include(static u => u.Vendor)
                          .OrderBy(static p => p.Vendor.Vendor)
                          .ThenBy(static p => p.Product)
                          .ThenBy(static p => p.ProductId)
                          .Select(static p => new UsbProductModel
                           {
                               ProductId   = p.ProductId,
                               ProductName = p.Product,
                               VendorId    = p.Vendor.Id,
                               VendorName  = p.Vendor.Vendor
                           })
                          .ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}