using Microsoft.AspNetCore.Identity;

namespace Aaru.Server.Components.Account;

sealed class IdentityUserAccessor(UserManager<IdentityUser> userManager, IdentityRedirectManager redirectManager)
{
    public async Task<IdentityUser> GetRequiredUserAsync(HttpContext context)
    {
        IdentityUser? user = await userManager.GetUserAsync(context.User);

        if(user is null)
        {
            redirectManager.RedirectToWithStatus("Account/InvalidUser",
                                                 $"Error: Unable to load user with ID '{userManager.GetUserId(context.User)}'.",
                                                 context);
        }

        return user;
    }
}