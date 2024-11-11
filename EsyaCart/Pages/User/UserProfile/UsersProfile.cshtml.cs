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
    
    public class UsersProfile : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersProfile> _logger;

        public UsersProfile(ILogger<UsersProfile> logger,AppDbContext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }

        public UserDetails userdetail {get; set;} = new UserDetails();
        public Account userAcc {get; set;} = new Account();
        public UserDetailsModelClass udmc {get; set;} = new UserDetailsModelClass();
        public int c_id {get;set;} = 0;
        public async Task<IActionResult> OnGet()
        {
           try
           {
             c_id = 1;
             userdetail = await _context.UserDetails.FirstOrDefaultAsync(u=>u.Accounts_Id==c_id);
             userAcc = await _context.Account.FirstOrDefaultAsync(u=>u.Account_Id==c_id);

             if(userdetail==null && userAcc==null)throw new Exception("User Details not found");
             udmc.UserName = userdetail.UserName;
             udmc.Area = userdetail.Area;
             udmc.Landmark = userdetail.Landmark;
             udmc.Pincode = userdetail.Pincode;
             return Page();
           }
           catch (Exception e)
           {
                Console.WriteLine(e.Message);
                TempData["msg"] = "Data not found";
                return Page();
           }

        }

    }
}