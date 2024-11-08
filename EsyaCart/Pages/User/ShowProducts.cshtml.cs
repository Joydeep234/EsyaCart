using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EsyaCart.Pages.User
{
    public class ShowProducts : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ShowProducts> _logger;

        public ShowProducts(ILogger<ShowProducts> logger,AppDbContext Dbcontext)
        {
            _logger = logger;
            _context = Dbcontext;
        }

        public List<Products> productsEntity {get; set;} = new List<Products>();
        public async Task OnGet(string productcatagoryID)
        {
            try
            {
                var categoryId = int.Parse(productcatagoryID);
                    productsEntity = await _context.Products
                                                .Where(p => p.Category_Id == categoryId)
                                                .ToListAsync();
                if(productsEntity.Count<0)throw new Exception("products not found");
            }
            catch (Exception e)
            {
                Console.WriteLine($"error=> {e.Message}");
            }
            
        }
    }
}