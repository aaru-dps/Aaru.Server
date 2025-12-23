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

using System.ComponentModel.DataAnnotations;
using Aaru.CommonTypes.Metadata;
using Microsoft.AspNetCore.Components;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.TestedMedia.Sequential;

public partial class Edit : ComponentBase
{
    private bool      _error;
    private EditModel _model = new();
    private bool      _notFound;
    private bool      _success;
    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await using DbContext  ctx    = await DbContextFactory.CreateDbContextAsync();
        TestedSequentialMedia? entity = await ctx.TestedSequentialMedia.FindAsync(Id);

        if(entity != null)
        {
            _model = new EditModel
            {
                Id             = entity.Id,
                Manufacturer   = entity.Manufacturer,
                Model          = entity.Model,
                MediumTypeName = entity.MediumTypeName
            };
        }
        else
            _notFound = true;
    }

    private async Task HandleValidSubmitAsync()
    {
        _success = false;
        _error   = false;

        try
        {
            await using DbContext  ctx    = await DbContextFactory.CreateDbContextAsync();
            TestedSequentialMedia? entity = await ctx.TestedSequentialMedia.FindAsync(_model.Id);

            if(entity == null)
            {
                _error = true;

                return;
            }

            entity.Manufacturer   = _model.Manufacturer;
            entity.Model          = _model.Model;
            entity.MediumTypeName = _model.MediumTypeName;
            await ctx.SaveChangesAsync();
            _success = true;
        }
        catch
        {
            _error = true;
        }
    }

    public class EditModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Manufacturer { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string MediumTypeName { get; set; } = string.Empty;
    }
}