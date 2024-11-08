using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Admin
{
    public class ProductsListModel : PageModel
    {
        public readonly AppDbContext _context;

        public ProductsListModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Products> productsList { get; set; }

        public void OnGet()
        {
            productsList = _context.Products.ToList();
        }
    }
}
