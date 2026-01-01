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
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Usb.Products;

public partial class List
{
    private int                   _currentPage = 1;
    private int                   _filteredCount;
    private bool                  _initialized;
    private bool                  _isLoading;
    private bool                  _isSearching;
    private List<UsbProductModel> _items    = new();
    private int                   _pageSize = 50;
    private Timer?                _searchDebounceTimer;
    private string                _searchTerm = string.Empty;
    private int                   _totalCount;
    private int                   _totalPages;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        // Get total count
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();
        _totalCount    = await ctx.UsbProducts.CountAsync();
        _filteredCount = _totalCount;

        // Load first page
        await LoadPageAsync();

        _initialized = true;

        StateHasChanged();
    }

    private async Task LoadPageAsync()
    {
        _isLoading = true;
        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        IQueryable<UsbProduct> query = ctx.UsbProducts.Include(static u => u.Vendor);

        // Apply search filter
        if(!string.IsNullOrWhiteSpace(_searchTerm))
        {
            string searchLower = _searchTerm.ToLower();

            query = query.Where(p => p.Vendor.Vendor.ToLower().Contains(searchLower) ||
                                     p.Product.ToLower().Contains(searchLower)       ||
                                     p.ProductId.ToString().Contains(searchLower));

            _filteredCount = await query.CountAsync();
        }
        else
            _filteredCount = _totalCount;

        // Calculate total pages
        _totalPages = (int)Math.Ceiling((double)_filteredCount / _pageSize);

        // Ensure current page is valid
        if(_currentPage > _totalPages && _totalPages > 0) _currentPage = _totalPages;

        if(_currentPage < 1) _currentPage = 1;

        // Load page data
        _items = await query.OrderBy(static p => p.Vendor.Vendor)
                            .ThenBy(static p => p.Product)
                            .ThenBy(static p => p.ProductId)
                            .Skip((_currentPage - 1) * _pageSize)
                            .Take(_pageSize)
                            .Select(static p => new UsbProductModel
                             {
                                 ProductId   = p.ProductId,
                                 ProductName = p.Product,
                                 VendorId    = p.Vendor.Id,
                                 VendorName  = p.Vendor.Vendor
                             })
                            .ToListAsync();

        _isLoading = false;
        StateHasChanged();
    }

    private async Task GoToPage(int page)
    {
        if(page < 1 || page > _totalPages || page == _currentPage) return;

        _currentPage = page;
        await LoadPageAsync();
    }

    private async Task HandlePageSizeChange(ChangeEventArgs e)
    {
        if(int.TryParse(e.Value?.ToString(), out int newSize))
        {
            _pageSize    = newSize;
            _currentPage = 1; // Reset to first page
            await LoadPageAsync();
        }
    }

    private void HandleSearchKeyUp(KeyboardEventArgs e)
    {
        // Debounce search
        _searchDebounceTimer?.Dispose();
        _isSearching = true;
        StateHasChanged();

        _searchDebounceTimer = new Timer(_ =>
                                         {
                                             InvokeAsync(async () =>
                                             {
                                                 _currentPage = 1; // Reset to first page on search
                                                 await LoadPageAsync();
                                                 _isSearching = false;
                                                 StateHasChanged();
                                             });
                                         },
                                         null,
                                         500,
                                         Timeout.Infinite);
    }

    private async Task ClearSearch()
    {
        _searchTerm  = string.Empty;
        _currentPage = 1;
        await LoadPageAsync();
    }

    private IEnumerable<int> GetPageNumbers()
    {
        var pages = new List<int>();

        if(_totalPages <= 7)
        {
            // Show all pages if 7 or fewer
            for(var i = 1; i <= _totalPages; i++) pages.Add(i);
        }
        else
        {
            // Always show first page
            pages.Add(1);

            if(_currentPage > 3) pages.Add(-1); // Ellipsis marker

            // Show pages around current page
            int start = Math.Max(2, _currentPage - 1);
            int end   = Math.Min(_totalPages     - 1, _currentPage + 1);

            for(int i = start; i <= end; i++) pages.Add(i);

            if(_currentPage < _totalPages - 2) pages.Add(-1); // Ellipsis marker

            // Always show last page
            pages.Add(_totalPages);
        }

        return pages.Where(p => p > 0); // Filter out ellipsis markers for now (can be enhanced)
    }

    public void Dispose()
    {
        _searchDebounceTimer?.Dispose();
    }
}