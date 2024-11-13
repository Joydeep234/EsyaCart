using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using EsyaCart.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EsyaCart.Pages.User.UserProfile
{
    public class UserOrderShow : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserOrderShow> _logger;
        public List<UserOrdersDetails> orderdet {get; set;} = new List<UserOrdersDetails>();
        public Reviews review {get;set;} =new Reviews();
        [BindProperty]
        public ReviewModelClass reviewmodel {get; set;} = new ReviewModelClass();
        public UserOrderShow(ILogger<UserOrderShow> logger,AppDbContext Dbcontext)
        {
            _logger = logger;
            _context = Dbcontext;
        }
         public string logoutchecksession {get;set;} = "";
        public int customerid { get; set; } 
        public async Task<IActionResult> OnGet()
        {
           try
           {
                logoutchecksession = HttpContext.Session.GetString("UserSessionId");
                customerid = HttpContext.Session.GetInt32("CustomerId")??0;
                if(logoutchecksession==null || customerid==0 ||customerid==null)return RedirectToPage("../../User/UserLogin");

             orderdet = await(from order in _context.Orders
             join ordItems in _context.OrderItems
             on order.Order_id equals ordItems.Order_Id
             join prod in _context.Products
             on ordItems.Product_Id equals prod.Product_Id
             where order.Customer_Id==customerid
             select new UserOrdersDetails{
                Product_id = prod.Product_Id,
                 ProdQuantity = ordItems.Quantity,
                 TotPrice = order.TotalPrice,
                 DeliveryStatus = order.isDelivered,
                 prodimgUrl = prod.ImageUrl,
                 ProdName = prod.ProductName,
                 orderdate = order.OrderedDate.ToLocalTime()
             }).ToListAsync();
             if(orderdet.Count<0)throw new Exception("Order is empty");

             return Page();
           }
           catch (Exception e)
           {
            Console.WriteLine(e.Message);
            TempData["err"] = "Didnot Order Anything";
            return Page();
           }
        }
        
        public async Task<IActionResult> OnPostEditReviews(int prodid){
            try
            {
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                logoutchecksession = HttpContext.Session.GetString("UserSessionId");
                customerid = HttpContext.Session.GetInt32("CustomerId")??0;
                Console.WriteLine($"productid is====>{prodid}");
                review = await _context.Reviews.FirstOrDefaultAsync(p=>p.Product_Id==prodid && p.Customer_Id==customerid);
                if(review==null){
                    if(!ModelState.IsValid)
                    {
                        throw new Exception("Error in data fetching from reviews input");
                    }
                    var rev = new Reviews{
                        Ratings = reviewmodel.Ratings,
                        Description = reviewmodel.Description,
                        Customer_Id = customerid,
                        Product_Id = prodid
                    };
                    await _context.Reviews.AddAsync(rev);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("../../User/Productanddetails", new {productID = prodid});
                }

                review.Description = reviewmodel.Description;
                review.Ratings = reviewmodel.Ratings;
                await _context.SaveChangesAsync();
                return RedirectToPage("../../User/Productanddetails", new {productID = prodid});
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToPage("../UserProfile/UsersProfile");
            }
        }
        
    }
    public class UserOrdersDetails{
        public int Product_id { get; set; }
        public int ProdQuantity {get;set;}
        public double TotPrice {get;set;}
        public bool DeliveryStatus {get;set;}
        public string prodimgUrl {get;set;}
        public string ProdName{get;set;}
        public DateTime orderdate {get;set;}
    }
}