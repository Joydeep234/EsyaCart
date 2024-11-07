using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EsyaCart.Models.Vendor;
using EsyaCart.Data.Context;
using System.ComponentModel.DataAnnotations;
using EsyaCart.Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace EsyaCart.Pages.Vendor
{
    public class VendorSignup : PageModel
    {
        private readonly AppDbContext _context;

        public VendorSignup(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VendorSignupModel vendorSignupModel { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                bool emailExist = await _context.Account.AnyAsync(p => p.Email == vendorSignupModel.Email);
                if (emailExist) {
                    ModelState.AddModelError("vendorSignupModel.Email", "Email already exist, try with another email address");
                }
                else
                {
                    Account vendorData = new Account
                    {
                        Email = vendorSignupModel.Email,
                        Contact_No = vendorSignupModel.Contact_No,
                        Password = vendorSignupModel.Password,
                        IsActive = false,
                        Account_type = "SELLER"
                    };

                    await _context.Account.AddAsync(vendorData);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("/Vendor/VendorLogin");
                }
            }
            return Page();
        }
    }
}
