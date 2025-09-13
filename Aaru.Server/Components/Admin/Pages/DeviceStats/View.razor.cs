using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.DeviceStats;

public partial class View
{
    private int      _deleteId;
    private Modal?   _deleteModal;
    bool             _initialized;
    List<DeviceStat> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.DeviceStats.OrderBy(static d => d.Manufacturer)
                          .ThenBy(static d => d.Model)
                          .ThenBy(static d => d.Bus)
                          .ToListAsync();

        _initialized = true;

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
        await using DbContext ctx  = await DbContextFactory.CreateDbContextAsync();
        DeviceStat?           stat = await ctx.DeviceStats.FindAsync(id);

        if(stat is not null)
        {
            ctx.DeviceStats.Remove(stat);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.DeviceStats.OrderBy(static d => d.Manufacturer)
                          .ThenBy(static d => d.Model)
                          .ThenBy(static d => d.Bus)
                          .ToListAsync();

        StateHasChanged();
    }
}