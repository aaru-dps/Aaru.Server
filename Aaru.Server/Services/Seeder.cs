// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Seeder.cs
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

using Microsoft.AspNetCore.Identity;
using DbContext = Aaru.Server.Database.DbContext;

namespace Aaru.Server.Services;

public static class Seeder
{
    public static async Task SeedAsync(DbContext ctx, IServiceProvider serviceProvider)
    {
        const string              email       = "claunia@claunia.com";
        var                       randChars   = new char[16];
        UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var                       rnd         = new Random();

        for(var i = 0; i < randChars.Length; i++) randChars[i] = (char)rnd.Next(32, 126);

        string password = new(randChars);

        if(await userManager.FindByEmailAsync(email) != null) return;

        var user = new IdentityUser
        {
            Email              = email,
            NormalizedEmail    = email,
            EmailConfirmed     = true,
            UserName           = email,
            NormalizedUserName = email
        };

        IdentityResult result = await userManager.CreateAsync(user, password);

        if(result.Succeeded) System.Console.WriteLine("Password is {0}, save it!", password);
    }
}