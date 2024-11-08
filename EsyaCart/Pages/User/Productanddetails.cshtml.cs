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
    public class Productanddetails : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<Productanddetails> _logger;

        public Productanddetails(ILogger<Productanddetails> logger,AppDbContext Dbcontext)
        {
            _logger = logger;
            _context = Dbcontext;
        }

        
        public Products ProductInfo { get; set; } = new Products();
        public List<UserReviewDetails> ProductReviewDetails { get; set; } = new List<UserReviewDetails>();

        public CartModelClass cartModel {get;set;} = new CartModelClass();
        public Cart cart { get; set; } = new Cart();
        public Cart cart2 { get; set; } = new Cart();
        public async Task OnGet(int productID)
        {
            try
            {
                cart2 = await _context.Cart.Where(c=>c.Customer_Id==1 && c.Product_Id == productID).FirstOrDefaultAsync();

                ProductInfo = await _context.Products.FirstOrDefaultAsync(p=>p.Product_Id==productID);
                if(ProductInfo==null)throw new Exception("Did not get Products");
                 ProductReviewDetails = await (from review in _context.Reviews
                                              join account in _context.Account 
                                              on review.Customer_Id equals account.Account_Id
                                              join userDetails in _context.UserDetails 
                                              on account.Account_Id equals userDetails.Accounts_Id
                                              where review.Product_Id == productID
                                              select new UserReviewDetails
                                              {
                                                  UserName = userDetails.UserName,
                                                  ReviewId = review.Review_Id,
                                                  Ratings = review.Ratings,
                                                  ReviewDescription = review.Description,
                                                  ProductId = review.Product_Id
                                              }).ToListAsync();
        

                if (ProductReviewDetails.Count<0)
                    throw new Exception("Review not available for this product");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }   
        public async Task<IActionResult> OnPostAddCartAndBuy(int prodId){
            try
            {
                cart = await _context.Cart.FirstOrDefaultAsync(p=>p.Product_Id == prodId);

                if(cart!=null){
                    cart.Quantity = cart.Quantity + 1;
                    await _context.SaveChangesAsync();
                    return RedirectToPage("../User/UserCart");
                }

                var newcartModel = new Cart(){
                    Customer_Id = 1,
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

        public async Task<IActionResult> OnPostAddToCart(int prodId){
            try
            {
                cart = await _context.Cart.FirstOrDefaultAsync(p=>p.Product_Id == prodId);

                if(cart!=null){
                    cart.Quantity = cart.Quantity + 1;
                    await _context.SaveChangesAsync();
                    return Redirect("~/User/Productanddetails?productID=" + prodId);

                }
               var newcartModel = new Cart(){
                    Customer_Id = 1,
                    Product_Id = prodId,
                    Quantity = 1
                };

                await _context.Cart.AddAsync(newcartModel);
                await _context.SaveChangesAsync();
                return Redirect("~/User/Productanddetails?productID=" + prodId);
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
                cart = await _context.Cart.FirstOrDefaultAsync(p=>p.Product_Id == prodId);
                
                if(cart.Quantity==1){
                    _context.Cart.Remove(cart);
                    await _context.SaveChangesAsync();
                    return Redirect("~/User/Productanddetails?productID=" + prodId);
                }

                if(cart!=null){
                    cart.Quantity = cart.Quantity - 1;
                    await _context.SaveChangesAsync();
                    return Redirect("~/User/Productanddetails?productID=" + prodId);

                }
               var newcartModel = new Cart(){
                    Customer_Id = 1,
                    Product_Id = prodId,
                    Quantity = 1
                };

                await _context.Cart.AddAsync(newcartModel);
                await _context.SaveChangesAsync();
                return RedirectToPage("../User/Productanddetails");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }
        }
    }

    public class UserReviewDetails
    {
        public string UserName { get; set; }
        public int ReviewId { get; set; }
        public float Ratings { get; set; }
        public string ReviewDescription { get; set; }
        public int ProductId { get; set; }
    }
}