using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Vendor
{
    public class ViewProducts : PageModel
    {

        private readonly AppDbContext _context;
        
        public ViewProducts(AppDbContext context)
        {
            _context = context;
        }

        public List<Products> productList;
        public void OnGet()
        {
            productList = _context.Products.ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            Console.WriteLine("This Function called");
            var currentProduct =  _context.Products.Find(id);
            Console.WriteLine(currentProduct);
            if (currentProduct != null)
            {
                _context.Products.Remove(currentProduct);
                _context.SaveChanges();
                return RedirectToPage();
            }
            else
            {
                return Page();
            }           
        }
    }
}
