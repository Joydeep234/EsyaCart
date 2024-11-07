using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EsyaCart.Pages.Admin
{
    public class VendorListPageModel : PageModel
    {
        public readonly AppDbContext _context;

        public VendorListPageModel(AppDbContext context) { _context = context; }

        public List<VendorDetails> VendorList { get; set; }

        public List<AllVendorDetails> AllVendorDetailsList {  get; set; } = new List<AllVendorDetails>();
        public async Task OnGet()
        {
            AllVendorDetailsList = await (from Account in _context.Account
                                          join VendorDetails in _context.VendorDetails
                                         on Account.Account_Id equals VendorDetails.Accounts_Id
                                          where Account.Account_type == "VENDOR"
                                          select new AllVendorDetails
                                          {
                                                VendorID = VendorDetails.VendorID,
                                                VendorName = VendorDetails.VendorName,
                                                Area = VendorDetails.Area,
                                                Landmark = VendorDetails.Landmark,
                                                Pincode = VendorDetails.Pincode,
                                                IsApproved = VendorDetails.IsApproved,
                                                IsActive= Account.IsActive,
                                                CreatedAt = Account.CreatedAt
                                          }).ToListAsync();
            /*VendorList = _context.VendorDetails.ToList();*/
        }
        public async Task<IActionResult> OnPostUpdateAsync(int VendorID, bool status)
        {
            
            var vendor2 = _context.VendorDetails.Find(VendorID);
            var account = _context.Account.Find(vendor2.Accounts_Id);
            

            /*var vendor = await _context.VendorDetails.FirstOrDefaultAsync(vd=>vd.VendorID == VendorID);*/
            if (vendor2 == null)
            {
                return NotFound();
            }
            else
            {
                account.IsActive = status;
                await _context.SaveChangesAsync();
                return RedirectToPage();
            }
        }
        /*public async Task<IActionResult> OnPostDeleteAsync(int VendorID)
        {
            var vendor = await _context.VendorDetails.FindAsync(VendorID);
            
            if (vendor == null)
            {
                return RedirectToPage();
            }
            else
            {
                 _context.VendorDetails.Remove(vendor);
                 
            }
            
            return RedirectToPage();
        }*/
    }
    public class AllVendorDetails
    {
        public int VendorID { get; set; }

        public string VendorName { get; set; }

        public bool IsApproved { get; set; }
        public string Area { get; set; }

        public string Landmark { get; set; }

        public int Pincode { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public string Account_type { get; set; }


    }
}
