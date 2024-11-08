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
    public class Dashboard : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<Dashboard> _logger;

        public List<Products> productsEntity {get;set;} = new List<Products>();
        public Dashboard(ILogger<Dashboard> logger,AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        public async Task OnGet()
        {
            try
            {
                productsEntity = await _context.Products.ToListAsync();
                if(productsEntity.Count<0)throw new Exception("Products not available");
            }
            catch (Exception e)
            {
               Console.WriteLine($"error=> {e.Message}");
            }
        }
    }
}