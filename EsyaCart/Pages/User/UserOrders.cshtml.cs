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
    public class UserOrders : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserOrders> _logger;

        public UserOrders(ILogger<UserOrders> logger,AppDbContext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }
        public List<Cart> usercart {get; set;} = new List<Cart>();
        public Orders Userorder {get;set;}  = new Orders();
        public List<CustomerProductsInCart> cartdata{ get; set; } = new List<CustomerProductsInCart>();
       
        public async Task<IActionResult> OnGet(double TotalPrice)
        {
            try
            {
                var order = new Orders{
                    Customer_Id = 1,
                    OrderedDate = DateTime.UtcNow,
                    TotalPrice = TotalPrice
                };

                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                int newOrderId = order.Order_id;


                usercart = await _context.Cart.Where(c=>c.Customer_Id==1).ToListAsync();

                cartdata = await (from products in _context.Products
                            join cart in _context.Cart
                            on products.Product_Id equals cart.Product_Id
                            where cart.Customer_Id==1
                            select new CustomerProductsInCart
                            {
                                ProductName = products.ProductName,
                                ProductImageUrl = products.ImageUrl,
                                ProductPrice = products.Price,
                                ProductQuantity = cart.Quantity,
                                Product_ID = cart.Product_Id
                            }).ToListAsync();

                foreach(var cart in cartdata){
                    var orderitem = new OrderItems{
                        Order_Id = newOrderId,
                        Product_Id = cart.Product_ID,
                        Quantity = cart.ProductQuantity,
                        Price = cart.ProductPrice,
                        TotalPrice = TotalPrice
                    };

                    await _context.OrderItems.AddAsync(orderitem);
                    await _context.SaveChangesAsync();
                }

                var allCartItems =await  _context.Cart.ToListAsync();
                if (allCartItems.Count>0)
                {
                    _context.Cart.RemoveRange(allCartItems);
                    await _context.SaveChangesAsync();
                }

                TempData["Data"] = TotalPrice.ToString();
                return Page();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["Data"] = "Unsuccessfull! Try Again";
                return RedirectToPage("../User/UserCart");
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
}