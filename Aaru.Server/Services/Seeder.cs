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