using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Aaru.Server;

public static class Seeder
{
    public static void Seed(AaruServerContext ctx, IServiceProvider serviceProvider)
    {
        string                    email       = "claunia@claunia.com";
        char[]                    randChars   = new char[16];
        UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var                       rnd         = new Random();

        for(int i = 0; i < randChars.Length; i++)
        {
            randChars[i] = (char)rnd.Next(32, 126);
        }

        string password = new(randChars);

        if(userManager.FindByEmailAsync(email).Result != null)
            return;

        var user = new IdentityUser
        {
            Email              = email,
            NormalizedEmail    = email,
            EmailConfirmed     = true,
            UserName           = email,
            NormalizedUserName = email
        };

        IdentityResult result = userManager.CreateAsync(user, password).Result;

        if(result.Succeeded)
        {
            System.Console.WriteLine("Password is {0}, save it!", password);
        }
    }
}