// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : ViewData.razor.cs
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

using System.Text;
using System.Web;
using Aaru.CommonTypes.Metadata;
using Aaru.Decoders.CD;
using Aaru.Helpers;
using Aaru.Server.Database.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Sector = Aaru.Decoders.CD.Sector;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Components.Admin.Pages.Gdrom;

public partial class ViewData
{
    bool                 _initialized;
    TestedMediaDataModel _model;
    bool                 _notFound;
    [Parameter]
    public string Data { get; set; } = string.Empty;
    [Parameter]
    public int Id { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _initialized = true;

        await using DbContext ctx = await DbContextFactory.CreateDbContextAsync();

        if(string.IsNullOrWhiteSpace(Data))
        {
            _notFound = true;

            return;
        }

        GdRomSwapDiscCapabilities? caps = await ctx.GdRomSwapDiscCapabilities.FirstOrDefaultAsync(m => m.Id == Id);

        if(caps == null) return;

        _model = new TestedMediaDataModel
        {
            TestedMediaId = Id,
            DataName      = Data
        };

        byte[]  buffer;
        var     sb      = new StringBuilder();
        byte[]  sector  = new byte[2352];
        byte[]  subq    = new byte[16];
        byte[]? fullsub = new byte[96];

        bool audio = true;
        bool pq    = false;
        bool rw    = false;

        switch(Data)
        {
            case nameof(caps.Lba0Data):
                buffer = caps.Lba0Data;
                audio  = false;

                break;
            case nameof(caps.Lba0ScrambledData):
                buffer = caps.Lba0ScrambledData;

                break;
            case nameof(caps.Lba44990Data):
                buffer = caps.Lba44990Data;
                audio  = false;

                break;
            case nameof(caps.Lba44990PqData):
                buffer = caps.Lba44990PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba44990RwData):
                buffer = caps.Lba44990RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba44990AudioData):
                buffer = caps.Lba44990AudioData;

                break;
            case nameof(caps.Lba44990AudioPqData):
                buffer = caps.Lba44990AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba44990AudioRwData):
                buffer = caps.Lba44990AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba45000Data):
                buffer = caps.Lba45000Data;
                audio  = false;

                break;
            case nameof(caps.Lba45000PqData):
                buffer = caps.Lba45000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba45000RwData):
                buffer = caps.Lba45000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba45000AudioData):
                buffer = caps.Lba45000AudioData;

                break;
            case nameof(caps.Lba45000AudioPqData):
                buffer = caps.Lba45000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba45000AudioRwData):
                buffer = caps.Lba45000AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba50000Data):
                buffer = caps.Lba50000Data;
                audio  = false;

                break;
            case nameof(caps.Lba50000PqData):
                buffer = caps.Lba50000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba50000RwData):
                buffer = caps.Lba50000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba50000AudioData):
                buffer = caps.Lba50000AudioData;

                break;
            case nameof(caps.Lba50000AudioPqData):
                buffer = caps.Lba50000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba50000AudioRwData):
                buffer = caps.Lba50000AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba100000Data):
                buffer = caps.Lba100000Data;
                audio  = false;

                break;
            case nameof(caps.Lba100000PqData):
                buffer = caps.Lba100000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba100000RwData):
                buffer = caps.Lba100000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba100000AudioData):
                buffer = caps.Lba100000AudioData;

                break;
            case nameof(caps.Lba100000AudioPqData):
                buffer = caps.Lba100000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba100000AudioRwData):
                buffer = caps.Lba100000AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba400000Data):
                buffer = caps.Lba400000Data;
                audio  = false;

                break;
            case nameof(caps.Lba400000PqData):
                buffer = caps.Lba400000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba400000RwData):
                buffer = caps.Lba400000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba400000AudioData):
                buffer = caps.Lba400000AudioData;

                break;
            case nameof(caps.Lba400000AudioPqData):
                buffer = caps.Lba400000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba400000AudioRwData):
                buffer = caps.Lba400000AudioRwData;
                rw     = true;

                break;
            case nameof(caps.Lba450000Data):
                buffer = caps.Lba450000Data;
                audio  = false;

                break;
            case nameof(caps.Lba450000PqData):
                buffer = caps.Lba450000PqData;
                audio  = false;
                pq     = true;

                break;
            case nameof(caps.Lba450000RwData):
                buffer = caps.Lba450000RwData;
                audio  = false;
                rw     = true;

                break;
            case nameof(caps.Lba450000AudioData):
                buffer = caps.Lba450000AudioData;

                break;
            case nameof(caps.Lba450000AudioPqData):
                buffer = caps.Lba450000AudioPqData;
                pq     = true;

                break;
            case nameof(caps.Lba450000AudioRwData):
                buffer = caps.Lba450000AudioRwData;
                rw     = true;

                break;
            default:
                return;
        }

        if(pq && buffer != null && buffer.Length % 2368 != 0) pq = false;

        if(rw && buffer != null && buffer.Length % 2448 != 0) rw = false;

        int blockSize = pq
                            ? 2368
                            : rw
                                ? 2448
                                : 2352;

        _model.RawDataAsHex = PrintHex.ByteArrayToHexArrayString(buffer);

        if(_model.RawDataAsHex != null)
            _model.RawDataAsHex = HttpUtility.HtmlEncode(_model.RawDataAsHex).Replace("\n", "<br/>");

        if(buffer == null)
        {
            _initialized = true;
            StateHasChanged();

            return;
        }

        for(int i = 0; i < buffer.Length; i += blockSize)
        {
            if(audio)
                sb.AppendLine("Audio or scrambled data sector.");
            else
            {
                Array.Copy(buffer, i, sector, 0, 2352);

                sb.AppendLine(Sector.Prettify(sector));
            }

            if(pq)
            {
                Array.Copy(buffer, i + 2352, subq, 0, 16);
                fullsub = Subchannel.ConvertQToRaw(subq);

                sb.AppendLine(GetPrettySub(fullsub));
            }
            else if(rw)
            {
                Array.Copy(buffer, i + 2352, fullsub, 0, 96);

                sb.AppendLine(GetPrettySub(fullsub));
            }

            sb.AppendLine();
        }

        _initialized = true;
        StateHasChanged();
    }

    static string GetPrettySub(byte[] sub)
    {
        byte[] deint = Subchannel.Deinterleave(sub);

        bool validP  = true;
        bool validRw = true;

        for(int i = 0; i < 12; i++)
        {
            if(deint[i] == 0x00 || deint[i] == 0xFF) continue;

            validP = false;

            break;
        }

        for(int i = 24; i < 96; i++)
        {
            if(deint[i] == 0x00) continue;

            validRw = false;

            break;
        }

        byte[] q = new byte[12];
        Array.Copy(deint, 12, q, 0, 12);

        return Subchannel.PrettifyQ(q, deint[21] > 0x10, 16, !validP, false, validRw);
    }
}