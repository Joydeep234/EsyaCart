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
        private readonly AppDbContext _context;

        public ViewProducts(AppDbContext context)
        {
            _context = context;
        }

          public List<Products> productList { get; set; } = new List<Products>();
        public List<Catagory> categoryList { get; set; } = new List<Catagory>();
        
		public List<ProductReviews> productReviews { get; set; }
		public async Task OnGetAsync()
        {
            productReviews = (from product in _context.Products
                              join review in _context.Reviews
                              on product.Product_Id equals review.Product_Id into reviewGroup
                              from review in reviewGroup.DefaultIfEmpty()  // Left join
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
        }


        public async Task<IActionResult> OnPostCategoryAsync(int categoryId)
        {
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
                                  from review in reviewGroup.DefaultIfEmpty() // Left join
                                  where product.Category_Id == categoryId
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
