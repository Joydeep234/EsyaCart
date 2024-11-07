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
        public void OnGet()
        {
        }
        public IActionResult OnPostAdminLogin() 
        {
            
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
