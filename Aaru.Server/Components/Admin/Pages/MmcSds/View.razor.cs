using Aaru.CommonTypes.Metadata;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.MmcSds;

public partial class View
{
    private int    _deleteId;
    private Modal? _deleteModal;
    bool           _initialized;
    List<MmcSd>    _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.MmcSd.ToListAsync();

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
        await using DbContext ctx   = await DbContextFactory.CreateDbContextAsync();
        MmcSd?                mmcSd = await ctx.MmcSd.FindAsync(id);

        if(mmcSd is not null)
        {
            ctx.MmcSd.Remove(mmcSd);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();
        _items = await ctx.MmcSd.ToListAsync();
        StateHasChanged();
    }
}