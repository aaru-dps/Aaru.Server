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