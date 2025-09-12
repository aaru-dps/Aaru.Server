using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;
using Version = Aaru.Server.Database.Models.Version;

namespace Aaru.Server.Components.Admin.Pages.Versions;

public partial class View
{
    private int    _deleteId;
    private Modal? _deleteModal;
    bool           _initialized;
    List<Version>  _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Versions.OrderBy(static v => v.Name).ToListAsync();

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
        await using DbContext ctx     = await DbContextFactory.CreateDbContextAsync();
        Version?              version = await ctx.Versions.FindAsync(id);

        if(version is not null)
        {
            ctx.Versions.Remove(version);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();
        _items = await ctx.Versions.OrderBy(static v => v.Name).ToListAsync();
        StateHasChanged();
    }
}