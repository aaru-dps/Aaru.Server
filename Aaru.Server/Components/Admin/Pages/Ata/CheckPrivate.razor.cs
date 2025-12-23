// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : CheckPrivate.razor.cs
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

using Aaru.CommonTypes.Structs.Devices.ATA;
using Aaru.Server.Database;

namespace Aaru.Server.Components.Admin.Pages.Ata;

public partial class CheckPrivate
{
    bool                                    _initialized;
    readonly List<CommonTypes.Metadata.Ata> _items = [];

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
        _items.Clear();
        byte[] tmp;

        await using DbContext _context = await DbContextFactory.CreateDbContextAsync();

        foreach(CommonTypes.Metadata.Ata ata in _context.Ata)
        {
            Identify.IdentifyDevice? id = ata.IdentifyDevice;

            if(id is null) continue;

            if(!string.IsNullOrWhiteSpace(((Identify.IdentifyDevice)id).SerialNumber) ||
               ((Identify.IdentifyDevice)id).WWN          != 0                        ||
               ((Identify.IdentifyDevice)id).WWNExtension != 0                        ||
               !string.IsNullOrWhiteSpace(((Identify.IdentifyDevice)id).MediaSerial))
            {
                _items.Add(ata);

                continue;
            }

            tmp = new byte[10];
            Array.Copy(ata.Identify, 121 * 2, tmp, 0, 10);

            if(tmp.All(static b => b > 0x20) && tmp.All(static b => b <= 0x5F))
            {
                _items.Add(ata);

                continue;
            }

            tmp = new byte[62];
            Array.Copy(ata.Identify, 129 * 2, tmp, 0, 62);

            if(tmp.All(static b => b > 0x20) && tmp.All(static b => b <= 0x5F))
            {
                _items.Add(ata);

                continue;
            }

            tmp = new byte[14];
            Array.Copy(ata.Identify, 161 * 2, tmp, 0, 14);

            if(tmp.All(static b => b > 0x20) && tmp.All(static b => b <= 0x5F))
            {
                _items.Add(ata);

                continue;
            }

            tmp = new byte[12];
            Array.Copy(ata.Identify, 224 * 2, tmp, 0, 12);

            if(tmp.All(static b => b > 0x20) && tmp.All(static b => b <= 0x5F))
            {
                _items.Add(ata);

                continue;
            }

            tmp = new byte[38];
            Array.Copy(ata.Identify, 236 * 2, tmp, 0, 38);

            if(tmp.All(static b => b > 0x20) && tmp.All(static b => b <= 0x5F)) _items.Add(ata);
        }
    }

    async Task ClearPrivateAll()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        foreach(CommonTypes.Metadata.Ata ata in ctx.Ata)
        {
            // Serial number
            for(int i = 0; i < 20; i++) ata.Identify[10 * 2 + i] = 0x20;

            // Media serial number
            for(int i = 0; i < 40; i++) ata.Identify[176 * 2 + i] = 0x20;

            // WWN and WWN Extension
            for(int i = 0; i < 16; i++) ata.Identify[108 * 2 + i] = 0;

            // We need to tell EFCore the entity has changed
            ctx.Update(ata);
        }

        await ctx.SaveChangesAsync();
    }

    async Task ClearReservedAll()
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        foreach(CommonTypes.Metadata.Ata ata in ctx.Ata)
        {
            // ReservedWords121
            for(int i = 0; i < 10; i++) ata.Identify[121 * 2 + i] = 0;

            // ReservedWords129
            for(int i = 0; i < 40; i++) ata.Identify[129 * 2 + i] = 0;

            // ReservedCFA
            for(int i = 0; i < 14; i++) ata.Identify[161 * 2 + i] = 0;

            // ReservedCEATA224
            for(int i = 0; i < 12; i++) ata.Identify[224 * 2 + i] = 0;

            // ReservedWords
            for(int i = 0; i < 14; i++) ata.Identify[161 * 2 + i] = 0;

            // We need to tell EFCore the entity has changed
            ctx.Update(ata);
        }

        await ctx.SaveChangesAsync();
    }

    async Task ClearPrivate(int id)
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        CommonTypes.Metadata.Ata? ata = ctx.Ata.FirstOrDefault(a => a.Id == id);

        if(ata is null) return;

        // Serial number
        for(int i = 0; i < 20; i++) ata.Identify[10 * 2 + i] = 0x20;

        // Media serial number
        for(int i = 0; i < 40; i++) ata.Identify[176 * 2 + i] = 0x20;

        // WWN and WWN Extension
        for(int i = 0; i < 16; i++) ata.Identify[108 * 2 + i] = 0;

        // We need to tell EFCore the entity has changed
        ctx.Update(ata);
        await ctx.SaveChangesAsync();

        await RefreshItemsAsync();
    }

    async Task ClearReserved(int id)
    {
        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        CommonTypes.Metadata.Ata? ata = ctx.Ata.FirstOrDefault(a => a.Id == id);

        if(ata is null) return;

        // ReservedWords121
        for(int i = 0; i < 10; i++) ata.Identify[121 * 2 + i] = 0;

        // ReservedWords129
        for(int i = 0; i < 40; i++) ata.Identify[129 * 2 + i] = 0;

        // ReservedCFA
        for(int i = 0; i < 14; i++) ata.Identify[161 * 2 + i] = 0;

        // ReservedCEATA224
        for(int i = 0; i < 12; i++) ata.Identify[224 * 2 + i] = 0;

        // ReservedWords
        for(int i = 0; i < 14; i++) ata.Identify[161 * 2 + i] = 0;

        // We need to tell EFCore the entity has changed
        ctx.Update(ata);
        await ctx.SaveChangesAsync();

        await RefreshItemsAsync();
    }
}