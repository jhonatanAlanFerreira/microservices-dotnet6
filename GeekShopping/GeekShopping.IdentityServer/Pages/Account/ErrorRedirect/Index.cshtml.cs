using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GeekShopping.IdentityServer.Pages.ErrorRedirect
{
    public class Index : PageModel
    {
        [AllowAnonymous]
        public void OnGet()
        {
        }
    }
}
