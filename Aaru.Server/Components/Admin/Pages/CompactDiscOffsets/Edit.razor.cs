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
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.CompactDiscOffsets;

public partial class Edit : ComponentBase
{
    private bool            _error;
    private OffsetEditModel _model = new();
    private bool            _notFound;
    private bool            _success;
    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await using DbContext ctx    = await DbContextFactory.CreateDbContextAsync();
        CompactDiscOffset?    entity = await ctx.CdOffsets.FindAsync(Id);

        if(entity != null)
        {
            _model = new OffsetEditModel
            {
                Id           = entity.Id,
                Manufacturer = entity.Manufacturer,
                Model        = entity.Model,
                Offset       = entity.Offset,
                Submissions  = entity.Submissions,
                Agreement    = entity.Agreement
            };
        }
        else
            _notFound = true;
    }

    private async Task HandleValidSubmit()
    {
        _success = false;
        _error   = false;

        try
        {
            await using DbContext ctx    = await DbContextFactory.CreateDbContextAsync();
            CompactDiscOffset?    entity = await ctx.CdOffsets.FindAsync(_model.Id);

            if(entity == null)
            {
                _error = true;

                return;
            }

            entity.Manufacturer = _model.Manufacturer;
            entity.Model        = _model.Model;
            entity.Offset       = _model.Offset;
            entity.Submissions  = _model.Submissions;
            entity.Agreement    = _model.Agreement;
            entity.ModifiedWhen = DateTime.UtcNow;
            await ctx.SaveChangesAsync();
            _success = true;
        }
        catch
        {
            _error = true;
        }
    }

    public class OffsetEditModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Manufacturer { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string Model { get; set; } = string.Empty;
        [Required]
        [Range(-10000, 10000)]
        public short Offset { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Submissions { get; set; }
        [Required]
        [Range(0, 100)]
        public float Agreement { get; set; }
    }
}