using Aaru.CommonTypes.Metadata;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.TestedMedia.Sequential;

public partial class View
{
    private int                 _deleteId;
    private Modal?              _deleteModal;
    bool                        _initialized;
    List<TestedSequentialMedia> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.TestedSequentialMedia.OrderBy(static m => m.Manufacturer)
                          .ThenBy(static m => m.Model)
                          .ThenBy(static m => m.MediumTypeName)
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
        await using DbContext  ctx   = await DbContextFactory.CreateDbContextAsync();
        TestedSequentialMedia? media = await ctx.TestedSequentialMedia.FindAsync(id);

        if(media is not null)
        {
            ctx.TestedSequentialMedia.Remove(media);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.TestedSequentialMedia.OrderBy(static m => m.Manufacturer)
                          .ThenBy(static m => m.Model)
                          .ThenBy(static m => m.MediumTypeName)
                          .ToListAsync();

        StateHasChanged();
    }
}