using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Vendor
{
    public class SessionClearerModel : PageModel
    {
        public IActionResult OnGetSessionClearer()
        {
            HttpContext.Session.Clear();
            return new JsonResult(new { success = true });
        }
    }
}
