using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.FireWire;

public partial class List
{
    private Modal?                      _consolidateModal;
    private int                         _deleteId;
    private Modal?                      _deleteModal;
    List<FireWireModel>                 _duplicates;
    bool                                _initialized;
    List<CommonTypes.Metadata.FireWire> _items;


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

        _items = await ctx.FireWire.OrderBy(static f => f.Manufacturer).ThenBy(static f => f.Product).ToListAsync();

        _duplicates = await ctx.FireWire.GroupBy(static x => new
                                {
                                    x.VendorID,
                                    x.ProductID,
                                    x.Manufacturer,
                                    x.Product,
                                    x.RemovableMedia
                                })
                               .Where(static x => x.Count() > 1)
                               .Select(static x => new FireWireModel
                                {
                                    VendorID       = x.Key.VendorID,
                                    ProductID      = x.Key.ProductID,
                                    Manufacturer   = x.Key.Manufacturer,
                                    Product        = x.Key.Product,
                                    RemovableMedia = x.Key.RemovableMedia
                                })
                               .ToListAsync();
    }

    Task ConsolidateDuplicatesAsync() => _consolidateModal?.ShowAsync();

    Task HideConsolidateModalAsync() => _consolidateModal?.HideAsync();

    async Task ConfirmConsolidateAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        foreach(FireWireModel duplicate in _duplicates)
        {
            CommonTypes.Metadata.FireWire? master = ctx.FireWire.FirstOrDefault(m => m.VendorID == duplicate.VendorID &&
                                                                                    m.ProductID ==
                                                                                    duplicate.ProductID &&
                                                                                    m.Manufacturer ==
                                                                                    duplicate.Manufacturer &&
                                                                                    m.Product ==
                                                                                    duplicate.Product &&
                                                                                    m.RemovableMedia ==
                                                                                    duplicate.RemovableMedia);

            if(master is null) continue;

            foreach(CommonTypes.Metadata.FireWire firewire in await ctx.FireWire
                                                                       .Where(m => m.VendorID == duplicate.VendorID  &&
                                                                                  m.ProductID == duplicate.ProductID &&
                                                                                  m.Manufacturer ==
                                                                                  duplicate.Manufacturer         &&
                                                                                  m.Product == duplicate.Product &&
                                                                                  m.RemovableMedia ==
                                                                                  duplicate.RemovableMedia)
                                                                       .Skip(1)
                                                                       .ToArrayAsync())
            {
                foreach(Device device in ctx.Devices.Where(d => d.FireWire.Id == firewire.Id)) device.FireWire = master;

                foreach(UploadedReport report in ctx.Reports.Where(d => d.FireWire.Id == firewire.Id))
                    report.FireWire = master;

                ctx.FireWire.Remove(firewire);
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
        await using DbContext          ctx      = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.FireWire? firewire = await ctx.FireWire.FindAsync(id);

        if(firewire is not null)
        {
            ctx.FireWire.Remove(firewire);
            await ctx.SaveChangesAsync();
        }
    }
}