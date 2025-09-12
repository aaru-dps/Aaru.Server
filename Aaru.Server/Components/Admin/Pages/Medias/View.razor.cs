using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Medias;

public partial class View
{
    private int    _deleteId;
    private Modal? _deleteModal;
    bool           _initialized;
    List<Media>    _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = (await ctx.Medias.ToListAsync()).OrderBy(static m => m.PhysicalType)
                                                 .ThenBy(static m => m.LogicalType)
                                                 .ThenBy(static m => m.Real)
                                                 .ToList();

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
        await using DbContext ctx         = await DbContextFactory.CreateDbContextAsync();
        Media?                mediaFormat = await ctx.Medias.FindAsync(id);

        if(mediaFormat is not null)
        {
            ctx.Medias.Remove(mediaFormat);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = (await ctx.Medias.ToListAsync()).OrderBy(static m => m.PhysicalType)
                                                 .ThenBy(static m => m.LogicalType)
                                                 .ThenBy(static m => m.Real)
                                                 .ToList();

        StateHasChanged();
    }
}