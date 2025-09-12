using Aaru.CommonTypes.Metadata;
using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Scsi.Ssc;

public partial class View
{
    private Modal?                 _consolidateModal;
    private int                    _deleteId;
    private Modal?                 _deleteModal;
    List<SscModel>                 _duplicates;
    bool                           _initialized;
    List<CommonTypes.Metadata.Ssc> _items;


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

        _items = await ctx.Ssc.OrderBy(static s => s.MinBlockLength)
                          .ThenBy(static s => s.MaxBlockLength)
                          .ThenBy(static s => s.BlockSizeGranularity)
                          .ToListAsync();

        _duplicates = await ctx.Ssc.GroupBy(static x => new
                                {
                                    x.BlockSizeGranularity,
                                    x.MaxBlockLength,
                                    x.MinBlockLength
                                })
                               .Where(static x => x.Count() > 1)
                               .Select(static x => new SscModel
                                {
                                    BlockSizeGranularity = x.Key.BlockSizeGranularity,
                                    MaxBlockLength       = x.Key.MaxBlockLength,
                                    MinBlockLength       = x.Key.MinBlockLength
                                })
                               .ToListAsync();
    }

    Task ConsolidateDuplicatesAsync() => _consolidateModal?.ShowAsync();

    Task HideConsolidateModalAsync() => _consolidateModal?.HideAsync();

    async Task ConfirmConsolidateAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        foreach(SscModel duplicate in _duplicates)
        {
            CommonTypes.Metadata.Ssc? master =
                ctx.Ssc.FirstOrDefault(m => m.BlockSizeGranularity == duplicate.BlockSizeGranularity &&
                                            m.MaxBlockLength       == duplicate.MaxBlockLength       &&
                                            m.MinBlockLength       == duplicate.MinBlockLength);

            if(master is null) continue;

            foreach(CommonTypes.Metadata.Ssc ssc in await ctx.Ssc
                                                             .Where(m => m.BlockSizeGranularity ==
                                                                         duplicate.BlockSizeGranularity               &&
                                                                         m.MaxBlockLength == duplicate.MaxBlockLength &&
                                                                         m.MinBlockLength == duplicate.MinBlockLength)
                                                             .Skip(1)
                                                             .ToArrayAsync())
            {
                foreach(TestedSequentialMedia media in ctx.TestedSequentialMedia.Where(d => d.SscId == ssc.Id))
                    media.SscId = master.Id;

                ctx.Ssc.Update(ssc);
                ctx.Ssc.Remove(ssc);
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
        CommonTypes.Metadata.Ssc? ssc = await ctx.Ssc.FindAsync(id);

        if(ssc is not null)
        {
            ctx.Ssc.Remove(ssc);
            await ctx.SaveChangesAsync();
        }
    }
}