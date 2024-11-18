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

        public bool approval;
        public int sellerId; // vendorID in Db context
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
            var sessionData = HttpContext.Session.GetString("VendorSessionId");

            if(sessionData != null)
            {
                int vendorSessionId = Convert.ToInt32(sessionData);
                var vendorData = _context.VendorDetails.Where(p => p.Accounts_Id == vendorSessionId).FirstOrDefault();
                approval = vendorData.IsApproved;
                sellerId = vendorData.VendorID;

                ViewData["CategoryList"] = await _context.Catagory
                    .Select(c => new SelectListItem
                    {
                        Value = c.Catagory_Id.ToString(),
                        Text = c.CatagoryName.ToString()
                    }).ToListAsync();

                return Page();
            }
            else
            {
                TempData["ToastMessage"] = "Please Login!";
                TempData["ToastType"] = "warning";
                return RedirectToPage("/Vendor/VendorLogin");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int vendorSessionId = Convert.ToInt32(HttpContext.Session.GetString("VendorSessionId"));
            var vendorData = _context.VendorDetails.Where(p => p.Accounts_Id == vendorSessionId).FirstOrDefault();
            approval = vendorData.IsApproved;
            sellerId = vendorData.VendorID;

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
                    Vendor_Id = sellerId,
                    ProductName = addProductModel.ProductName,
                    Price = addProductModel.Price,
                    Quantity = addProductModel.Quantity,
                    Description = addProductModel.Descirption,
                    IsActive = false,
                    ImageUrl = fileName
                };

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                TempData["ToastMessage"] = "New Product Added";
                TempData["ToastType"] = "success";

                return RedirectToPage("/Vendor/ViewProducts");
                
            }
            else
            {
                TempData["ToastMessage"] = "Please, Fill all the feilds";
                TempData["ToastType"] = "warning";
                return RedirectToPage();
            }
        }
    }
}
