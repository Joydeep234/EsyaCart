using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Numerics;


namespace EsyaCart.Pages.Vendor
{
    public class ViewProducts : PageModel
    {

        public bool approval;
        public int sellerId;
        private readonly AppDbContext _context;

        public ViewProducts(AppDbContext context)
        {
            _context = context;
        }

        public List<Catagory> categoryList { get; set; } = new List<Catagory>();

        public List<ProductReviews> productReviews { get; set; } = new List<ProductReviews>();
		public async Task<IActionResult> OnGetAsync()
        {
            var sessionData = HttpContext.Session.GetString("VendorSessionId");
            if(sessionData != null)
            {
                int vendorSessionId = Convert.ToInt32(sessionData);
                var vendorData = _context.VendorDetails.Where(p => p.Accounts_Id == vendorSessionId).FirstOrDefault();
                approval = vendorData.IsApproved;
                sellerId = vendorData.VendorID;

                productReviews = (from product in _context.Products
                                  join review in _context.Reviews
                                  on product.Product_Id equals review.Product_Id into reviewGroup
                                  where (product.Vendor_Id == sellerId)
                                  select new ProductReviews
                                  {
                                      Vendor_Id = product.Vendor_Id,
                                      Product_Id = product.Product_Id,
                                      ProductName = product.ProductName,
                                      Price = product.Price,
                                      Quantity = product.Quantity,
                                      IsActive = product.IsActive,
                                      Description = product.Description,
                                      ImageUrl = product.ImageUrl,
                                      Ratings = reviewGroup.Any() ? reviewGroup.Average(r => r.Ratings) : 0  
                                  }).ToList();

                categoryList = await _context.Catagory.ToListAsync();
                return Page();
            }
            else
            {
                return RedirectToPage("/Vendor/VendorLogin");
            }
        }

        public async Task<IActionResult> OnPostCategoryAsync(int categoryId)
        {

            int vendorSessionId = Convert.ToInt32(HttpContext.Session.GetString("VendorSessionId"));

            var vendorData = _context.VendorDetails.Where(p => p.Accounts_Id == vendorSessionId).FirstOrDefault();
            approval = vendorData.IsApproved;
            sellerId = vendorData.VendorID;

            if (categoryId == 0)
            {
                return Page();
            }
            else
            {
                categoryList = await _context.Catagory.ToListAsync();

                productReviews = (from product in _context.Products
                                      join review in _context.Reviews
                                      on product.Product_Id equals review.Product_Id into reviewGroup
                                      where (product.Category_Id == categoryId && product.Vendor_Id == sellerId)
                                      select new ProductReviews
                                      {
                                          Vendor_Id = product.Vendor_Id,
                                          Product_Id = product.Product_Id,
                                          ProductName = product.ProductName,
                                          Price = product.Price,
                                          Quantity = product.Quantity,
                                          IsActive = product.IsActive,
                                          Description = product.Description,
                                          ImageUrl = product.ImageUrl,
                                          Ratings = reviewGroup.Any() ? reviewGroup.Average(r => r.Ratings) : 0  // Average rating if there are reviews
                                      }).ToList();



                if (productReviews.Count == 0)
                {
					productReviews.Clear();
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnPostEnableAsync(int productId)
        {
            var currentProduct = await _context.Products.FindAsync(productId);
            if (currentProduct != null)
            {
                currentProduct.IsActive = true;
                await _context.SaveChangesAsync();
                TempData["ToastMessage"] = "Product is live now!";
                TempData["ToastType"] = "info";
                return RedirectToPage();
            }
            else
            {
                TempData["ToastMessage"] ="Error while fetching product data";
                TempData["ToastType"] = "error";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDisableAsync(int productId)
        {
            var currentProduct = await _context.Products.FindAsync(productId);
            if(currentProduct != null && currentProduct.IsActive == true)
            {
                currentProduct.IsActive = false;
                await _context.SaveChangesAsync();

                TempData["ToastMessage"] = "Product Disabled for selling!";
                TempData["ToastType"] = "warning";
                return RedirectToPage();
            }
            else
            {
                return Page();
            }
        }
    }

    public class ProductReviews
    {

        public int Vendor_Id { get; set; }
        public int Product_Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string ImageUrl { get; set; }

        public float Ratings { get; set; }
    }
}
