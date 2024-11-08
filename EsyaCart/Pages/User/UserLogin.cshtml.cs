using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EsyaCart.Data.Context;
using EsyaCart.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EsyaCart.Pages.User
{
    
    public class UserLogin : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserLogin> _logger;

        public UserLogin(ILogger<UserLogin> logger,AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }

        [BindProperty]
        public LoginModelClass LInput { get; set; } = new LoginModelClass();
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
               else
               {
                   var author=await _context.Account.Where(p=>p.Email==LInput.Email && p.Password==LInput.Password).FirstOrDefaultAsync();
                   if (author == null)
                   {
                       throw new Exception("Enter Correct Login Details");
                   }
                   return RedirectToPage("../User/Dashboard");
               }
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