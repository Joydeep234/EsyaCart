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
        public List<Catagory> catagoryList {get;set;} = new List<Catagory>();
        public List<Products> productsEntity {get;set;} = new List<Products>();
        public Dashboard(ILogger<Dashboard> logger,AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }
        public Cart cart { get; set; } = new Cart();
        public int customerid { get; set; }
        public string logoutchecksession {get;set;} = "";
        public async Task OnGet(string catagoryName)
        {
            try
            {
                logoutchecksession = HttpContext.Session.GetString("UserSessionId");
                Console.WriteLine($"session id===>{logoutchecksession}");
                catagoryList = await _context.Catagory.ToListAsync();
                if(catagoryList.Count < 0)throw new Exception("catagory not there");
                if(catagoryName==null)
                productsEntity = await _context.Products.ToListAsync();
                else{
                    productsEntity = await (from prod in _context.Products
                                            join cat in _context.Catagory
                                            on prod.Category_Id equals cat.Catagory_Id
                                            where cat.CatagoryName == catagoryName
                                            select prod).ToListAsync();
                }
                if(productsEntity.Count<0)throw new Exception("Products not available");
            }
            catch (Exception e)
            {
               Console.WriteLine($"error=> {e.Message}");
            }
        }

        public async Task<IActionResult> OnPostAddCartAndBuy(int prodId){
            try
            {
                logoutchecksession = HttpContext.Session.GetString("UserSessionId");
                customerid = HttpContext.Session.GetInt32("CustomerId")??0;
                if(logoutchecksession == null)return RedirectToPage("../User/UserLogin");
                cart = await _context.Cart.FirstOrDefaultAsync(p=>p.Product_Id == prodId);

                if(cart!=null){
                    cart.Quantity = cart.Quantity + 1;
                    await _context.SaveChangesAsync();
                    return RedirectToPage("../User/UserCart");
                }

                var newcartModel = new Cart(){
                    Customer_Id = customerid,
                    Product_Id = prodId,
                    Quantity = 1
                };

                await _context.Cart.AddAsync(newcartModel);
                await _context.SaveChangesAsync();

                return RedirectToPage("../User/UserCart");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }
        }
    }
}