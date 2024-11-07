using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using EsyaCart.Models.Vendor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Vendor
{
	public class VendorHome : PageModel
    {

        private readonly AppDbContext _context;

        public VendorHome(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VendorDetailsModel vendorDetailsModel { get; set; }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync() {

            if(ModelState.IsValid)
            {
                VendorDetails vendorDetails = new VendorDetails
                {
                    VendorName = vendorDetailsModel.VendorName,
                    Area = vendorDetailsModel.Area,
                    Landmark = vendorDetailsModel.Landmark,
                    Pincode = vendorDetailsModel.Pincode,
                    Accounts_Id = 4
                };

                await _context.AddAsync(vendorDetails);
                await _context.SaveChangesAsync();

            }

            return Page();
        }
    }
}
