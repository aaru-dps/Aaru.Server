// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : List.razor.cs
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
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Usb.Devices;

public partial class List
{
    private Modal?                         _consolidateModal;
    private int                            _deleteId;
    private Modal?                         _deleteModal;
    private List<UsbModel>                 _duplicates = new();
    private bool                           _initialized;
    private List<CommonTypes.Metadata.Usb> _items      = new();
    private string                         _searchTerm = string.Empty;

    private IEnumerable<CommonTypes.Metadata.Usb> FilteredItems
    {
        get
        {
            if(string.IsNullOrWhiteSpace(_searchTerm)) return _items;

            return _items.Where(item =>
                                    (item.Manufacturer?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    (item.Product?.Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ??
                                     false) ||
                                    item.VendorID.ToString()
                                        .Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                    item.ProductID.ToString()
                                        .Contains(_searchTerm, StringComparison.OrdinalIgnoreCase) ||
                                    item.Id.ToString().Contains(_searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }


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
                    if(!master.Descriptors.SequenceEqual(slave.Descriptors))
                        continue;

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