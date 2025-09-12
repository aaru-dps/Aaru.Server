using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.TestedMedia;

public partial class List
{
    private int                            _deleteId;
    private Modal?                         _deleteModal;
    bool                                   _initialized;
    List<CommonTypes.Metadata.TestedMedia> _items;


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

        _items = await ctx.TestedMedia.OrderBy(static m => m.Manufacturer)
                          .ThenBy(static m => m.Model)
                          .ThenBy(static m => m.MediumTypeName)
                          .ThenBy(static m => m.MediaIsRecognized)
                          .ThenBy(static m => m.LongBlockSize)
                          .ThenBy(static m => m.BlockSize)
                          .ThenBy(static m => m.Blocks)
                          .ToListAsync();
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
        await using DbContext             ctx   = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.TestedMedia? media = await ctx.TestedMedia.FindAsync(id);

        if(media is not null)
        {
            ctx.TestedMedia.Remove(media);
            await ctx.SaveChangesAsync();
        }
    }
}