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
    public class UserCart : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserCart> _logger;

        public UserCart(ILogger<UserCart> logger,AppDbContext Dbcontext)
        {
            _logger = logger;
            _context = Dbcontext;
        }

        public List<CustomerProductsInCart> cartdata{ get; set; } = new List<CustomerProductsInCart>();
        public Cart cart { get; set; } = new Cart();
        public Products products { get; set; } = new Products();
        public string logoutchecksession {get;set;} = "";
        public int customerid { get; set; } 

        public int setbtnlologin { get; set; } = 1;
        public async Task<IActionResult> OnGet()
        {
            try
            {
                logoutchecksession = HttpContext.Session.GetString("UserSessionId");
                customerid = HttpContext.Session.GetInt32("CustomerId")??0;
                if(customerid==0 || customerid==null ||logoutchecksession==null){
                    setbtnlologin = 0;
                    return Page();
                }
                setbtnlologin=1;
                cartdata = await (from products in _context.Products
                join cart in _context.Cart
                on products.Product_Id equals cart.Product_Id
                where cart.Customer_Id==customerid
                select new CustomerProductsInCart
                {
                    ProductName = products.ProductName,
                    ProductImageUrl = products.ImageUrl,
                    ProductPrice = products.Price,
                    ProductQuantity = cart.Quantity,
                    Product_ID = cart.Product_Id
                }).ToListAsync();
                if(cartdata.Count<0)throw new Exception("Cart is Empty");
                return Page();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }
        }

         public async Task<IActionResult> OnPostAddToCart(int prodId){
            try
            {
                logoutchecksession = HttpContext.Session.GetString("UserSessionId");
                customerid = HttpContext.Session.GetInt32("CustomerId")??0;
                cart = await _context.Cart.FirstOrDefaultAsync(p=>p.Product_Id == prodId);

                if(cart!=null){
                    products = await _context.Products.FirstOrDefaultAsync(p=>p.Product_Id==prodId);
                    if(products.Quantity<=cart.Quantity){
                        TempData["datamsg"] = "Maximum Products Available To You";
                        return Redirect("~/User/UserCart");
                    }
                    cart.Quantity = cart.Quantity + 1;
                        await _context.SaveChangesAsync();
                        return Redirect("~/User/UserCart");
                }
               var newcartModel = new Cart(){
                    Customer_Id = customerid,
                    Product_Id = prodId,
                    Quantity = 1
                };

                await _context.Cart.AddAsync(newcartModel);
                await _context.SaveChangesAsync();
                return Redirect("~/User/UserCart");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostDecreasefromCart(int prodId){
             try
            {
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                logoutchecksession = HttpContext.Session.GetString("UserSessionId");
                customerid = HttpContext.Session.GetInt32("CustomerId")??0;
                cart = await _context.Cart.FirstOrDefaultAsync(p=>p.Product_Id == prodId);
                
                if(cart.Quantity==1){
                    _context.Cart.Remove(cart);
                    await _context.SaveChangesAsync();
                    return Redirect("~/User/UserCart");
                }

                if(cart!=null){
                    cart.Quantity = cart.Quantity - 1;
                    await _context.SaveChangesAsync();
                    return Redirect("~/User/UserCart");

                }
               var newcartModel = new Cart(){
                    Customer_Id = customerid,
                    Product_Id = prodId,
                    Quantity = 1
                };

                await _context.Cart.AddAsync(newcartModel);
                await _context.SaveChangesAsync();
                // return RedirectToPage("~/User/UserCart");
                return Page();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }
        }

        public async Task<IActionResult> OnPostRemovefromCart(int prodId){
           try
           {
             cart = await _context.Cart.FirstOrDefaultAsync(p=>p.Product_Id == prodId);
                
                    _context.Cart.Remove(cart);
                    await _context.SaveChangesAsync();
                    return Redirect("~/User/UserCart");
           }
           catch (Exception e)
           {
            Console.WriteLine(e.Message);
             return Redirect("~/User/UserCart");
           }
        }
    }
    public class CustomerProductsInCart
    {
        public string ProductName { get; set; }
        public string ProductImageUrl {get; set; }


        public double ProductPrice { get; set; }
        public int ProductQuantity {get;set;}
        public int Product_ID {get;set;}
    }
}