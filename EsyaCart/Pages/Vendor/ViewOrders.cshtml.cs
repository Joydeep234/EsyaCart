using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace EsyaCart.Pages.Vendor
{
    public class ViewOrders : PageModel
    {
        private readonly AppDbContext _context;

        public bool approval;

        public int sellerId;

        public ViewOrders(AppDbContext context)
        {
            _context = context;
        }

        public List<SellerOrders> sellerOrders { get; set; } = new List<SellerOrders>(); 

        public async Task<IActionResult> OnGetAsync()
        {
            var sessionData = HttpContext.Session.GetString("VendorSessionId");

            if(sessionData != null)
            {
                int vendorSessionId = Convert.ToInt32(sessionData);
                var vendorData = _context.VendorDetails.Where(p => p.Accounts_Id == vendorSessionId).FirstOrDefault();

                approval = vendorData.IsApproved;
                sellerId = vendorData.VendorID;


				sellerOrders = (from product in _context.Products
									where product.Vendor_Id == sellerId
									join orderItem in _context.OrderItems on product.Product_Id equals orderItem.Product_Id into ordersGroup
									select new SellerOrders
									{
										ProductName = product.ProductName,
										OrdersCount = ordersGroup.Any() ? ordersGroup.Sum(orderItem => orderItem.Quantity) : 0

									}).ToList();

				return Page();
            }
            else
            {
                return RedirectToPage("/Vendor/VendorLogin");
            }
        }
    }

    public class SellerOrders
    {
        public string ProductName;

        public int OrdersCount;

        public float Ratings;
    }
}
