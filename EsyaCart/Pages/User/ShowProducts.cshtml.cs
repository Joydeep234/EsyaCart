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
        [BindProperty]
        public SliderModelClass slidermodel { get; set; } = new SliderModelClass();
        public List<Products> productsEntity {get; set;} = new List<Products>();
        public string logoutchecksession {get;set;} = "";
        public double maxprice {get;set;} = 0;
        public string cat_id {get;set;}
        public Catagory catagory  {get;set;}
        public Cart cart { get; set; } = new Cart();
        public int customerid { get; set; }
        public async Task OnGet(string productcatagoryID,double pricerange)
        {
            try
            {
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                logoutchecksession = HttpContext.Session.GetString("UserSessionId");
                cat_id = productcatagoryID;
                var categoryId = int.Parse(productcatagoryID);
                if(pricerange>0){
                    productsEntity = await _context.Products
                                                .Where(p => p.Category_Id == categoryId && p.Price <= pricerange)
                                                .ToListAsync();
                }
                 else{
                    productsEntity = await _context.Products
                                                .Where(p => p.Category_Id == categoryId)
                                                .ToListAsync();
                 }   
                    catagory = await _context.Catagory.FirstOrDefaultAsync(c=>c.Catagory_Id==categoryId);
                    maxprice = await _context.Products.Where(p => p.Category_Id == categoryId).MaxAsync(p=>p.Price);
                if(productsEntity.Count<0)throw new Exception("products not found");
            }
            catch (Exception e)
            {
                Console.WriteLine($"error=> {e.Message}");
            }
            
        }

        public async Task<IActionResult> OnPostOnFilterRange(int catid){
            try
            {
                if(slidermodel.SelectedPrice>0){
                    return RedirectToPage("../User/ShowProducts",new {productcatagoryID = catid,pricerange = slidermodel.SelectedPrice});
                }
                return Page();
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
               return Page();
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