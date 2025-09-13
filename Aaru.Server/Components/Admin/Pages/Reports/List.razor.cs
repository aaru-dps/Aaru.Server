using Aaru.Server.Database.Models;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Reports;

public partial class List
{
    private int          _deleteId;
    private Modal?       _deleteModal;
    bool                 _initialized;
    List<UploadedReport> _items;


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

        _items = await ctx.Reports.OrderBy(static r => r.Manufacturer)
                          .ThenBy(static r => r.Model)
                          .ThenBy(static r => r.Revision)
                          .ThenBy(static r => r.CompactFlash)
                          .ThenBy(static r => r.Type)
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
        await using DbContext ctx    = await DbContextFactory.CreateDbContextAsync();
        UploadedReport?       report = await ctx.Reports.FindAsync(id);

        if(report is not null)
        {
            ctx.Reports.Remove(report);
            await ctx.SaveChangesAsync();
        }
    }
}