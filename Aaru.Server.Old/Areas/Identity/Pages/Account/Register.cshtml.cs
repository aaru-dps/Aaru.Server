using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aaru.Server.Old.Areas.Identity.Pages.Account;

[AllowAnonymous]
public sealed class RegisterModel : PageModel
{
    public IActionResult OnGetAsync(string returnUrl = null) => RedirectToPage("Login");

    public IActionResult OnPostAsync(string returnUrl = null) => RedirectToPage("Login");
}