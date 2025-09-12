using Aaru.CommonTypes.Metadata;
using BlazorBootstrap;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.SupportedDensities;

public partial class View
{
    private int            _deleteId;
    private Modal?         _deleteModal;
    bool                   _initialized;
    List<SupportedDensity> _items;

    /// <inheritdoc />
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        StateHasChanged();

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.SupportedDensity.OrderBy(static d => d.Organization)
                          .ThenBy(static d => d.Name)
                          .ThenBy(static d => d.Description)
                          .ThenBy(static d => d.Capacity)
                          .ThenBy(static d => d.PrimaryCode)
                          .ThenBy(static d => d.SecondaryCode)
                          .ThenBy(static d => d.BitsPerMm)
                          .ThenBy(static d => d.Width)
                          .ThenBy(static d => d.Tracks)
                          .ThenBy(static d => d.DefaultDensity)
                          .ThenBy(static d => d.Writable)
                          .ThenBy(static d => d.Duplicate)
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
        await DeleteVersionAsync(_deleteId);
        await HideDeleteModal();
        await RefreshItemsAsync();
    }

    private async Task DeleteVersionAsync(int id)
    {
        await using DbContext ctx              = await DbContextFactory.CreateDbContextAsync();
        SupportedDensity?     supportedDensity = await ctx.SupportedDensity.FindAsync(id);

        if(supportedDensity is not null)
        {
            ctx.SupportedDensity.Remove(supportedDensity);
            await ctx.SaveChangesAsync();
        }
    }

    private async Task RefreshItemsAsync()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        _items = await ctx.SupportedDensity.OrderBy(static d => d.Organization)
                          .ThenBy(static d => d.Name)
                          .ThenBy(static d => d.Description)
                          .ThenBy(static d => d.Capacity)
                          .ThenBy(static d => d.PrimaryCode)
                          .ThenBy(static d => d.SecondaryCode)
                          .ThenBy(static d => d.BitsPerMm)
                          .ThenBy(static d => d.Width)
                          .ThenBy(static d => d.Tracks)
                          .ThenBy(static d => d.DefaultDensity)
                          .ThenBy(static d => d.Writable)
                          .ThenBy(static d => d.Duplicate)
                          .ToListAsync();

        StateHasChanged();
    }
}