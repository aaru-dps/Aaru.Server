using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Usb.Devices;

public partial class List
{
    private Modal?                 _consolidateModal;
    private int                    _deleteId;
    private Modal?                 _deleteModal;
    List<UsbModel>                 _duplicates;
    bool                           _initialized;
    List<CommonTypes.Metadata.Usb> _items;


    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await RefreshItemsAsync();

        _initialized = true;

        StateHasChanged();
    }

    async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Usb.OrderBy(static u => u.Manufacturer)
                          .ThenBy(static u => u.Product)
                          .ThenBy(static u => u.VendorID)
                          .ThenBy(static u => u.ProductID)
                          .ToListAsync();

        _duplicates = await ctx.Usb.GroupBy(static x => new
                                {
                                    x.Manufacturer,
                                    x.Product,
                                    x.VendorID,
                                    x.ProductID
                                })
                               .Where(static x => x.Count() > 1)
                               .Select(static x => new UsbModel
                                {
                                    Manufacturer = x.Key.Manufacturer,
                                    Product      = x.Key.Product,
                                    VendorID     = x.Key.VendorID,
                                    ProductID    = x.Key.ProductID
                                })
                               .ToListAsync();
    }

    Task ConsolidateDuplicatesAsync() => _consolidateModal?.ShowAsync();

    Task HideConsolidateModalAsync() => _consolidateModal?.HideAsync();

    async Task ConfirmConsolidateAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        foreach(UsbModel duplicate in _duplicates)
        {
            CommonTypes.Metadata.Usb? master = ctx.Usb.FirstOrDefault(m => m.Manufacturer == duplicate.Manufacturer &&
                                                                           m.Product      == duplicate.Product      &&
                                                                           m.VendorID     == duplicate.VendorID     &&
                                                                           m.ProductID    == duplicate.ProductID);

            if(master is null) continue;

            foreach(CommonTypes.Metadata.Usb slave in await ctx.Usb
                                                               .Where(m => m.Manufacturer == duplicate.Manufacturer &&
                                                                           m.Product      == duplicate.Product      &&
                                                                           m.VendorID     == duplicate.VendorID     &&
                                                                           m.ProductID    == duplicate.ProductID)
                                                               .Skip(1)
                                                               .ToArrayAsync())
            {
                if(slave.Descriptors != null && master.Descriptors != null)
                {
                    if(!master.Descriptors.SequenceEqual(slave.Descriptors)) continue;
                }

                foreach(Device device in ctx.Devices.Where(d => d.USB.Id == slave.Id)) device.USB = master;

                foreach(UploadedReport report in ctx.Reports.Where(d => d.USB.Id == slave.Id)) report.USB = master;

                if(master.Descriptors is null && slave.Descriptors != null)
                {
                    master.Descriptors = slave.Descriptors;
                    ctx.Usb.Update(master);
                }

                ctx.Usb.Remove(slave);
            }
        }

        await ctx.SaveChangesAsync();

        await RefreshItemsAsync();

        StateHasChanged();
    }

    private async Task ShowDeleteModal(int id)
    {
        _deleteId = id;
        if(_deleteModal != null) await _deleteModal.ShowAsync();
    }

    private async Task HideDeleteModal()
    {
        if(_deleteModal != null) await _deleteModal.HideAsync();
    }

    private async Task ConfirmDelete()
    {
        await DeleteAsync(_deleteId);
        await HideDeleteModal();
        await RefreshItemsAsync();
    }

    private async Task DeleteAsync(int id)
    {
        await using DbContext     ctx = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.Usb? usb = await ctx.Usb.FindAsync(id);

        if(usb is not null)
        {
            ctx.Usb.Remove(usb);
            await ctx.SaveChangesAsync();
        }
    }
}