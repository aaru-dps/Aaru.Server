// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : UploadedReportDetails.cs
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

using Aaru.CommonTypes.Metadata;

namespace Aaru.Server.Database.Models;

public class UploadedReportDetails
{
    public UploadedReport?             Report                 { get; set; }
    public List<int>                   SameAll                { get; set; }
    public List<int>                   SameButManufacturer    { get; set; }
    public List<int>                   ReportAll              { get; set; }
    public List<int>                   ReportButManufacturer  { get; set; }
    public int                         ReadCapabilitiesId     { get; set; }
    public List<TestedMedia>           TestedMedias           { get; set; }
    public List<TestedSequentialMedia> TestedSequentialMedias { get; set; }
}