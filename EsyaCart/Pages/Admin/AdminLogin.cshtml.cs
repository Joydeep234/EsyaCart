using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using EsyaCart.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Admin
{
    public class AdminLoginModel : PageModel
    {
        public readonly AppDbContext _context;

        public AdminLoginModel(AppDbContext context) {  _context = context; }

        [BindProperty]
        public AdminLogin AdminLogin { get; set; }

        

      /*  public Account Account { get; set; }*/
        public IActionResult OnGet()
        {
            var sessionId = HttpContext.Session.GetString("AdminSessionId");
            if (sessionId != null)
            {
                return RedirectToPage("/Admin/AdminHomePage");
            }
            return Page();
        }
        public IActionResult OnPostAdminLogin() 
        {
            HttpContext.Session.SetString("AdminSessionId", "AdminLoginByLikhith@123");
            Console.WriteLine($"Admin session {HttpContext.Session.GetString("AdminSessionId")}");
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var authUser = _context.Account.Where(p => p.Email == AdminLogin.Email && p.Password == AdminLogin.Password).FirstOrDefault();

                if (authUser != null)
                {
                    return RedirectToPage("/Admin/AdminHomePage");
                }
                else
                {
                    return Page();
                }
            }
        }
    }
}
