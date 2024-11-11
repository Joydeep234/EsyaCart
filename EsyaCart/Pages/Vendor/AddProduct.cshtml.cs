using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using EsyaCart.Models.Vendor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EsyaCart.Pages.Vendor
{
    public class AddProduct : PageModel
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AddProduct(AppDbContext context, IWebHostEnvironment _env)
        {
            _context = context;
            _environment = _env;
        }

        [BindProperty]
        public AddProductModel addProductModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {   
            ViewData["CategoryList"] = await _context.Catagory
                .Select(c => new SelectListItem
                {
                    Value = c.Catagory_Id.ToString(),
                    Text = c.CatagoryName.ToString()
                }).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["CategoryList"] = await _context.Catagory
            .Select(c => new SelectListItem
            {
               Value = c.Catagory_Id.ToString(),
               Text = c.CatagoryName
            }).ToListAsync();


            if (ModelState.IsValid)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(addProductModel.ImageUrl.FileName);

                string filePath = Path.Combine(_environment.WebRootPath, "Uploads", fileName);

                var fileStream = new FileStream(filePath, FileMode.Create);

                await addProductModel.ImageUrl.CopyToAsync(fileStream);

                var selectedCategoryId = addProductModel.Category_Id;

                Products product = new Products
                {  
                    Category_Id = selectedCategoryId,
                    Vendor_Id = 1,
                    ProductName = addProductModel.ProductName,
                    Price = addProductModel.Price,
                    Quantity = addProductModel.Quantity,
                    Description = addProductModel.Descirption,
                    IsActive = false,
                    ImageUrl = fileName
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                Console.WriteLine("product added successfully");

                return RedirectToPage("/Vendor/ViewProducts");
                
            }
            return Page();
        }
    }
}
