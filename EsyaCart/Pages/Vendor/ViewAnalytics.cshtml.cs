using EsyaCart.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace EsyaCart.Pages.Vendor
{
    public class ViewAnalytics : PageModel
    {
		private readonly AppDbContext _context;

		public ViewAnalytics(AppDbContext context)
		{
			_context = context;
		}
		public int sellerId { get; set; }

		public int TodaysOrders { get; set; }

		public int TotalOrders { get; set; }

		public int TodaysRevenue { get; set; }

		public int TotalRevenue { get; set; }


		public void OnGet()
        {
			var sessionData = HttpContext.Session.GetString("VendorSessionId");
			if(sessionData != null)
			{
				var today = DateTime.Today;

                int vendorSessionId = Convert.ToInt32(sessionData);
                var vendordata = _context.VendorDetails.Where(p => p.Accounts_Id == vendorSessionId).FirstOrDefault();

				sellerId = vendordata.VendorID;

                var vendorAnalytics = (from product in _context.Products
									   where product.Vendor_Id == sellerId
									   join orderItem in _context.OrderItems on product.Product_Id equals orderItem.Product_Id
									   join order in _context.Orders on orderItem.Order_Id equals order.Order_id
									   group new { product, orderItem, order } by product.Vendor_Id into vendorGroup
									   select new
									   {
										   VendorId = vendorGroup.Key,

										   // Count of distinct orders placed today
										   TodaysOrders = vendorGroup
											   .Where(x => x.order.OrderedDate.Date == today)
											   .Select(x => x.order.Order_id)
											   .Distinct()
											   .Count(),

										   // Total count of distinct orders
										   TotalOrders = vendorGroup
											   .Select(x => x.order.Order_id)
											   .Distinct()
											   .Count(),
										   
										   TodaysRevenue = vendorGroup
											   .Where(x => x.order.OrderedDate.Date == today)
											   .Sum(x => x.orderItem.Quantity * x.product.Price),

										   
										   TotalRevenue = vendorGroup
											   .Sum(x => x.orderItem.Quantity * x.product.Price),

									   }).FirstOrDefault();

				Console.WriteLine(TodaysOrders + " " + TotalOrders + " " + TodaysRevenue + " " + TotalRevenue);
			}

		}
	}
}
