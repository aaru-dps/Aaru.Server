// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Edit.razor.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ License ] --------------------------------------------------------------
//
//     This library is free software; you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as
//     published by the Free Software Foundation; either version 2.1 of the
//     License, or (at your option) any later version.
//
//     This library is distributed in the hope that it will be useful, but
//     WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//     Lesser General Public License for more details.
//
//     You should have received a copy of the GNU Lesser General Public
//     License along with this library; if not, see <http://www.gnu.org/licenses/>.
//
// ----------------------------------------------------------------------------
// Copyright © 2011-2026 Natalia Portillo
// ****************************************************************************/

using Microsoft.AspNetCore.Components;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.FireWire;

public partial class Edit : ComponentBase
{
    DbContext _db;

    CommonTypes.Metadata.FireWire _fireWire = new();
    bool                          _isLoaded;
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _db = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.FireWire? entity = await _db.FireWire.FindAsync(Id);
        if(entity != null) _fireWire          = entity;
        _isLoaded = true;
    }

    async Task HandleValidSubmit()
    {
        _db.Update(_fireWire);
        await _db.SaveChangesAsync();
        Navigation.NavigateTo("/admin/firewire");
    }

    void GoBack()
    {
        Navigation.NavigateTo("/admin/firewire");
    }
}