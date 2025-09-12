using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Usb.Vendors;

public partial class Details
{
    bool                  _initialized;
    UsbVendor?            _model;
    List<UsbProductModel> _products;
    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _model = await ctx.UsbVendors.FirstOrDefaultAsync(m => m.VendorId == Id);

        _products = await ctx.UsbProducts.Where(p => p.Vendor.VendorId == Id)
                             .OrderBy(static p => p.Product)
                             .ThenBy(static p => p.ProductId)
                             .Select(static p => new UsbProductModel
                              {
                                  ProductId   = p.ProductId,
                                  ProductName = p.Product
                              })
                             .ToListAsync();

        _initialized = true;

        StateHasChanged();
    }
}