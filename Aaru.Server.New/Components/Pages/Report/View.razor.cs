using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.New.Components.Pages.Report;

public partial class View
{
    bool   _initialized;
    bool   _notFound;
    bool   accordionItem1Visible;
    string _pageTitle { get; set; } = "Aaru Device Report";
    [CascadingParameter]
    HttpContext HttpContext { get; set; } = default!;
    [Parameter]
    public int Id { get;        set; }
    public Item? UsbItem { get; set; }

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

        Device? report = await ctx.Devices.Include(static deviceReportV2 => deviceReportV2.USB)
                                  .FirstOrDefaultAsync(d => d.Id == Id);

        if(report is null)
        {
            _notFound                       = true;
            HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

            return;
        }

        _pageTitle = $"Aaru Device Report for {report.Manufacturer} {report.Model} {report.Revision}";

        if(report.USB != null)
        {
            string? usbVendorDescription  = null;
            string? usbProductDescription = null;

            UsbProduct? dbProduct = await ctx.UsbProducts.Include(static usbProduct => usbProduct.Vendor)
                                             .FirstOrDefaultAsync(p => p.ProductId       == report.USB.ProductID &&
                                                                       p.Vendor.VendorId == report.USB.VendorID);

            if(dbProduct is null)
            {
                UsbVendor? dbVendor = await ctx.UsbVendors.FirstOrDefaultAsync(v => v.VendorId == report.USB.VendorID);

                if(dbVendor is not null) usbVendorDescription = dbVendor.Vendor;
            }
            else
            {
                usbProductDescription = dbProduct.Product;
                usbVendorDescription  = dbProduct.Vendor.Vendor;
            }

            UsbItem = new Item
            {
                Manufacturer = report.USB.Manufacturer,
                Product      = report.USB.Product,
                VendorDescription =
                    usbVendorDescription != null
                        ? $"0x{report.USB.VendorID:x4} ({usbVendorDescription})"
                        : $"0x{report.USB.VendorID:x4}",
                ProductDescription = usbProductDescription != null
                                         ? $"0x{report.USB.ProductID:x4} ({usbProductDescription})"
                                         : $"0x{report.USB.ProductID:x4}"
            };
        }

        _initialized = true;

        StateHasChanged();
    }
}

public class Item
{
    public string Manufacturer;
    public string Product;
    public string ProductDescription;
    public string VendorDescription;
}