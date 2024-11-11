using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using EsyaCart.Models.Vendor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Vendor
{
    public class EditProduct : PageModel
    {
        private readonly AppDbContext _context;

        public EditProduct(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Products products { get; set; }

        [BindProperty]
        public EditProductModel editProducts { get; set; }


        public async Task<IActionResult> OnGetAsync(int productId)
        {
            var OldData = await _context.Products.FindAsync(productId);
            if (OldData != null)
            {
                editProducts = new EditProductModel
                {
                    ProductName = OldData.ProductName,
                    Price = OldData.Price,
                    Quantity = OldData.Quantity,
                    Description = OldData.Description
                };
                return Page();
            }
            return Page();
        }

        public IActionResult OnPost(int productId)
        {
            var UpdatedData = _context.Products.Find(productId);
            if (UpdatedData != null)
            {
                UpdatedData.ProductName = editProducts.ProductName;
                UpdatedData.Price = editProducts.Price;
                UpdatedData.Quantity = editProducts.Quantity;
                UpdatedData.Description = editProducts.Description;

                _context.SaveChanges();

                TempData["Success"] = "Details Updated Successfully";

                return RedirectToPage("/Vendor/ViewProducts");
            }
            else
            {
                return Page();
            }
        }
    }
}
