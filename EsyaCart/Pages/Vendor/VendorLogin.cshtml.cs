using EsyaCart.Data.Context;
using EsyaCart.Models.Vendor;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EsyaCart.Pages.Vendor
{
    public class VedorLogin : PageModel
    {
        private readonly AppDbContext _context;
        public VedorLogin(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VendorLoginModel vendorLoginModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var validUser = await _context.Account
                    .SingleOrDefaultAsync(x => x.Email == vendorLoginModel.Email && x.Password == vendorLoginModel.Password);

                if (validUser != null)
                {

                    TempData["Success"] = "User " + validUser.Email + " Logedin SUccessfull";
                    Console.WriteLine(TempData["Success"]);
                    return RedirectToPage("/Vendor/VendorHome");
                }
                else
                {
                    ModelState.AddModelError("vendorLoginModel.Password", "Invalid credentials");
                }
            }
            return Page();
        }

    }
}
