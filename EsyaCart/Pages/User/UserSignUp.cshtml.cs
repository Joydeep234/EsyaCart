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
    public class UserSignUp : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserSignUp> _logger;

        public UserSignUp(ILogger<UserSignUp> logger,AppDbContext Dbcontext)
        {
            _logger = logger;
            _context = Dbcontext;
        }

        [BindProperty]
        public UserSignUpModelClass USInput { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(){
            try
            {
                 if (!ModelState.IsValid)
                {
                    throw new Exception("Invalid Details");
                }
                
                bool ckeckMailExist = await _context.Account.AnyAsync(u=>u.Email==USInput.EmailID);
                if(ckeckMailExist)throw new Exception("Email already exist");
                if (USInput.Password != USInput.ConformPassword)
                {
                   throw new Exception("Password and Conform Password both need to same"); 
                }
                    var newUser = new Account
                    {
                        Email = USInput.EmailID,
                        Contact_No=USInput.ContactNumber,
                        Password = USInput.Password,
                        Account_type = "USER",
                        
                    };
                   await _context.Account.AddAsync(newUser);
                   await _context.SaveChangesAsync();
                    
                    return RedirectToPage("../User/UserLogin");
            }            
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["error"] = e.Message.ToString();
                return Page();
            }
        }
    }
}