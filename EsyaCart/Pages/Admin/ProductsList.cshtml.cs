using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace EsyaCart.Pages.Admin
{
    public class ProductsListModel : PageModel
    {
        public readonly AppDbContext _context;

        public ProductsListModel(AppDbContext context)
        {
            _context = context;
        }

        //public List<sellerDetails> Details { get; set; }

        public List<sellerDetails> productList;
        public async Task OnGet()
        {
            //productList = await _context.Products.ToListAsync();

            productList = await (from VendorDetails in _context.VendorDetails
                                 join Products in _context.Products
                                on VendorDetails.VendorID equals Products.Vendor_Id
                                 select new sellerDetails
                                 {
                                     VendorID = VendorDetails.VendorID,
                                     VendorName = VendorDetails.VendorName,
                                     ProductName = Products.ProductName,
                                     Price = Products.Price,
                                     Description = Products.Description,
                                     ImageUrl = Products.ImageUrl
                                 }).ToListAsync();
        }


    }
    public class sellerDetails
    {
        public int VendorID { get; set; }

        public string VendorName { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }

        public string ImageUrl { get; set; }

    }
}
