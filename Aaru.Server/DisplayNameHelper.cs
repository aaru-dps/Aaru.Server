// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : DisplayNameHelper.cs
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

using System.ComponentModel;
using System.Reflection;

namespace Aaru.Server;

public static class DisplayNameHelper
{
    public static string GetDisplayName(Type type, string propertyName)
    {
        PropertyInfo? prop = type.GetProperty(propertyName);

        if(prop is null) return propertyName;

        DisplayNameAttribute? displayAttr = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                                                .OfType<DisplayNameAttribute>()
                                                .FirstOrDefault();

        return displayAttr?.DisplayName ?? propertyName;
    }
}