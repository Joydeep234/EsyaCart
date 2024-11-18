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
        public int cat_id {get;set;}
        public Catagory catagory  {get;set;}
        public  string qstring {get;set;}
        public Cart cart { get; set; } = new Cart();
        public int customerid { get; set; }
        public async Task OnGet(int productcatagoryID,double pricerange,string queryString)
        {
            try
            {
                Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                Response.Headers["Pragma"] = "no-cache";
                Response.Headers["Expires"] = "0";
                logoutchecksession = HttpContext.Session.GetString("UserSessionId");

                
                if(!string.IsNullOrEmpty(queryString) && pricerange<1){ //string ,no range
                    double price = 0;
                    bool isPriceQuery = double.TryParse(queryString, out price);

                    productsEntity = await (from p in _context.Products
                                    join c in _context.Catagory on p.Category_Id equals c.Catagory_Id
                                    where c.CatagoryName.Contains(queryString) ||
                                        c.CategoryDescription.Contains(queryString) ||
                                        p.ProductName.Contains(queryString) ||
                                        p.Description.Contains(queryString) ||
                                        (isPriceQuery && p.Price <= price)
                                    select p).ToListAsync();
                    if(productsEntity.Count<0)throw new Exception("products not found");
                    catagory = await _context.Catagory.FirstOrDefaultAsync(c=>c.CatagoryName==queryString);
                    if(catagory != null)cat_id = catagory.Catagory_Id;
                    qstring = queryString;
                        maxprice = await _context.Products.MaxAsync(p=>p.Price);
                    return;
                }
                else if(pricerange>0 && queryString!=null){

                    productsEntity = await (from p in _context.Products
                            join c in _context.Catagory on p.Category_Id equals c.Catagory_Id
                            where (c.CatagoryName.Contains(queryString) ||
                                   c.CategoryDescription.Contains(queryString) ||
                                   p.ProductName.Contains(queryString) ||
                                   p.Description.Contains(queryString)) &&
                                  p.Price <= pricerange
                            select p).ToListAsync();
                    if(productsEntity.Count<0)throw new Exception("products not found");
                    catagory = await _context.Catagory.FirstOrDefaultAsync(c=>c.CatagoryName==queryString);
                    if(catagory != null)cat_id = catagory.Catagory_Id;
                    qstring = queryString;
                    maxprice = await _context.Products.MaxAsync(p => p.Price);
                    return;
                }else if(productcatagoryID==0 && pricerange<1 && queryString==null) /*no string,no price,no cat*/
                { 
                    productsEntity = await _context.Products.ToListAsync();
                    if(productsEntity.Count<0)throw new Exception("products not found");
                    maxprice = await _context.Products.MaxAsync(p=>p.Price);
                    return;
                }else if(productcatagoryID==0 && pricerange>0 && queryString==null){ //no string and price
                    productsEntity = await _context.Products
                                                .Where(p=>p.Price <= pricerange)
                                                .ToListAsync();
                    if(productsEntity.Count<0)throw new Exception("products not found");
                    maxprice = await _context.Products.MaxAsync(p=>p.Price);
                    return;
                }

                else if(productcatagoryID>0 && pricerange<1 && queryString==null){ //cat only
                    productsEntity = await _context.Products
                                                .Where(p => p.Category_Id == productcatagoryID)
                                                .ToListAsync();
                    if(productsEntity.Count<0)throw new Exception("products not found");
                    catagory = await _context.Catagory.FirstOrDefaultAsync(c=>c.Catagory_Id==productcatagoryID);
                    if(catagory != null)cat_id = catagory.Catagory_Id;
                    
                    maxprice = await _context.Products.Where(p => p.Category_Id == productcatagoryID).MaxAsync(p=>p.Price);
                    return;
                }
                else if(productcatagoryID>0 && pricerange>0 && queryString==null){ //cat and range
                    productsEntity = await _context.Products
                                                .Where(p => p.Category_Id == productcatagoryID && p.Price <= pricerange)
                                                .ToListAsync();
                    if(productsEntity.Count<0)throw new Exception("products not found");
                    catagory = await _context.Catagory.FirstOrDefaultAsync(c=>c.Catagory_Id==productcatagoryID);
                    if(catagory != null)cat_id = catagory.Catagory_Id;
                    
                    maxprice = await _context.Products.Where(p => p.Category_Id == productcatagoryID).MaxAsync(p=>p.Price);
                    return;
                }
                //  else {
                //     productsEntity = await _context.Products
                //                                 .Where(p => p.Category_Id == productcatagoryID)
                //                                 .ToListAsync();
                //                                 if(productsEntity.Count<0)throw new Exception("products not found");
                //     return;
                //  }   
              
            }
            catch (Exception e)
            {
                Console.WriteLine($"error=> {e.Message}");
            }
            
        }

        public async Task<IActionResult> OnPostOnFilterRange(int catid,string qstring2){
            try
            {
                if(slidermodel.SelectedPrice>0){
                    return RedirectToPage("../User/ShowProducts",new {productcatagoryID = catid,pricerange = slidermodel.SelectedPrice,queryString = qstring2});
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