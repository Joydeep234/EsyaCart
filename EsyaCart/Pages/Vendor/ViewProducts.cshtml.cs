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
        private readonly AppDbContext _context;

        public ViewProducts(AppDbContext context)
        {
            _context = context;
        }

        public List<Catagory> categoryList { get; set; } = new List<Catagory>();

        public List<ProductReviews> productReviews { get; set; } = new List<ProductReviews>();
		public async Task<IActionResult> OnGetAsync()
        {
			var vendorData = _context.VendorDetails.Where(p => p.Accounts_Id == 4).FirstOrDefault();
			approval = vendorData.IsApproved;

            productReviews = (from product in _context.Products
                              join review in _context.Reviews
                              on product.Product_Id equals review.Product_Id into reviewGroup
                              from review in reviewGroup.DefaultIfEmpty()  // Left join
                              where product.Vendor_Id == 1 // 9 = sessions id of vendor
                              select new ProductReviews
                              {
                                  Product_Id = product.Product_Id,
                                  ProductName = product.ProductName,
                                  Price = product.Price,
                                  Quantity = product.Quantity,
                                  IsActive = product.IsActive,
                                  Description = product.Description,
                                  ImageUrl = product.ImageUrl,
                                  Ratings = review == null ? 0 : review.Ratings  // Default to 0 if no review
                              }).ToList();

            categoryList = await _context.Catagory.ToListAsync();
            return Page();
        }


        public async Task<IActionResult> OnPostCategoryAsync(int categoryId)
        {
            var vendorData = _context.VendorDetails.Where(p => p.Accounts_Id == 4).FirstOrDefault();
            approval = vendorData.IsApproved;

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
                                      where (product.Category_Id == categoryId && product.Vendor_Id == 1)
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
