using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Scsi.Cdrom;

public partial class List
{
    private int           _deleteId;
    private Modal?        _deleteModal;
    bool                  _initialized;
    List<MmcModelForView> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Mmc.Where(static m => m.ModeSense2AData != null)
                          .Select(static m => new MmcModelForView
                           {
                               Id         = m.Id,
                               FeaturesId = m.FeaturesId,
                               DataLength = m.ModeSense2AData.Length
                           })
                          .Concat(ctx.Mmc.Where(static m => m.ModeSense2AData == null)
                                     .Select(static m => new MmcModelForView
                                      {
                                          Id         = m.Id,
                                          FeaturesId = m.FeaturesId,
                                          DataLength = 0
                                      }))
                          .OrderBy(static m => m.Id)
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
        await using DbContext     ctx = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.Mmc? mmc = await ctx.Mmc.FindAsync(id);

        if(mmc is not null)
        {
            ctx.Mmc.Remove(mmc);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.Mmc.Where(static m => m.ModeSense2AData != null)
                          .Select(static m => new MmcModelForView
                           {
                               Id         = m.Id,
                               FeaturesId = m.FeaturesId,
                               DataLength = m.ModeSense2AData.Length
                           })
                          .Concat(ctx.Mmc.Where(static m => m.ModeSense2AData == null)
                                     .Select(static m => new MmcModelForView
                                      {
                                          Id         = m.Id,
                                          FeaturesId = m.FeaturesId,
                                          DataLength = 0
                                      }))
                          .OrderBy(static m => m.Id)
                          .ToListAsync();

        StateHasChanged();
    }
}