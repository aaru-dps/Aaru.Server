// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Hash.cs
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

using System.Security.Cryptography;

namespace Aaru.Server.Core;

public static class Hash
{
    public static string Sha512(byte[] data)
    {
        byte[] hash;

        using(var sha = new SHA512Managed())
        {
            sha.Initialize();
            hash = sha.ComputeHash(data);
        }

        char[] chars = new char[hash.Length * 2];

        int j = 0;

        foreach(byte b in hash)
        {
            int nibble1 = (b & 0xF0) >> 4;
            int nibble2 = b & 0x0F;

            nibble1 += nibble1 > 9 ? 0x57 : 0x30;
            nibble2 += nibble2 > 9 ? 0x57 : 0x30;

            chars[j++] = (char)nibble1;
            chars[j++] = (char)nibble2;
        }

        return new string(chars);
    }
}