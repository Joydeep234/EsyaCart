using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Admin
{
    public class LogOutPageModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            HttpContext.Session.Clear();
            Console.WriteLine($"logout session : {HttpContext.Session.GetString("AdminSessionId")}");
            return RedirectToPage("/Admin/AdminLogin");
        }

       
    }
}
