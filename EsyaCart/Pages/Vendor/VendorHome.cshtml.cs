using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using EsyaCart.Models.Vendor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Vendor
{
	public class VendorHome : PageModel
    {
		public bool approval;


		private readonly AppDbContext _context;

        public VendorHome(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public VendorDetailsModel vendorDetailsModel { get; set; }
        public void OnGet()
        {
			var vendorData = _context.VendorDetails.Where(p => p.Accounts_Id == 4).FirstOrDefault();
			approval = vendorData.IsApproved;
			

		}

        public async Task<IActionResult> OnPostAsync() {

			var UpdatedData = _context.VendorDetails.Where(p => p.Accounts_Id == 4).FirstOrDefault();
			// 9 = Session Id
			if (UpdatedData != null)
			{
				UpdatedData.VendorName = vendorDetailsModel.VendorName;
				UpdatedData.Area= vendorDetailsModel.Area;
				UpdatedData.Landmark = vendorDetailsModel.Landmark;
				UpdatedData.Pincode = vendorDetailsModel.Pincode;
				UpdatedData.IsApproved = vendorDetailsModel.isApproved;

				_context.SaveChanges();

				TempData["Success"] = "Details Updated Successfully";

				return RedirectToPage();
			}
			else
			{
				return Page();
			}
		}
    }
}
