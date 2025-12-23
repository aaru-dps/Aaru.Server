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
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.TestedMedia;

public partial class Edit : ComponentBase
{
    DbContext db;
    bool      isLoaded;

    TestedMediaViewModel testedMedia = new();
    [Parameter]
    public int Id { get; set; }

    [Inject]
    public IDbContextFactory<DbContext> DbContextFactory { get; set; }

    [Inject]
    public NavigationManager Navigation { get; set; }

    protected override async Task OnInitializedAsync()
    {
        db = await DbContextFactory.CreateDbContextAsync();
        CommonTypes.Metadata.TestedMedia? entity = await db.TestedMedia.FindAsync(Id);

        if(entity != null)
        {
            testedMedia = new TestedMediaViewModel
            {
                Id                = Id,
                Manufacturer      = entity.Manufacturer,
                Model             = entity.Model,
                MediumTypeName    = entity.MediumTypeName,
                MediaIsRecognized = entity.MediaIsRecognized,
                Blocks            = entity.Blocks,
                BlockSize         = entity.BlockSize,
                LongBlockSize     = entity.LongBlockSize
            };
        }

        isLoaded = true;
    }

    protected async Task HandleValidSubmit()
    {
        CommonTypes.Metadata.TestedMedia? entity = await db.TestedMedia.FindAsync(Id);

        if(entity != null)
        {
            entity.Manufacturer      = testedMedia.Manufacturer;
            entity.Model             = testedMedia.Model;
            entity.MediumTypeName    = testedMedia.MediumTypeName;
            entity.MediaIsRecognized = testedMedia.MediaIsRecognized;
            entity.Blocks            = testedMedia.Blocks;
            entity.BlockSize         = testedMedia.BlockSize;
            entity.LongBlockSize     = testedMedia.LongBlockSize;
            await db.SaveChangesAsync();
        }

        Navigation.NavigateTo("/admin/tested-media");
    }

    protected void GoBack()
    {
        Navigation.NavigateTo("/admin/tested-media");
    }

    public class TestedMediaViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string Model { get;             set; }
        public string MediumTypeName    { get; set; }
        public bool   MediaIsRecognized { get; set; }
        [Range(0, long.MaxValue)]
        public ulong? Blocks { get; set; }
        [Range(0, int.MaxValue)]
        public uint? BlockSize { get; set; }
        [Range(0, int.MaxValue)]
        public uint? LongBlockSize { get; set; }
    }
}