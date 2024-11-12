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
    public class Edituserprofile : PageModel
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UsersProfile> _logger;

        public Edituserprofile(ILogger<UsersProfile> logger,AppDbContext dbcontext)
        {
            _logger = logger;
            _context = dbcontext;
        }

        public UserDetails userdetail {get; set;} = new UserDetails();
        public Account userAcc {get; set;} = new Account();
        [BindProperty]
        public UserDetailsModelClass udmc {get; set;} = new UserDetailsModelClass();
        public async Task<IActionResult> OnGet(int id)
        {
           try
           {
             userdetail = await _context.UserDetails.FirstOrDefaultAsync(u=>u.Accounts_Id==id);
             userAcc = await _context.Account.FirstOrDefaultAsync(u=>u.Account_Id==id);

             if(userdetail==null && userAcc==null)throw new Exception("User Details not found");
             return Page();
           }
           catch (Exception e)
           {
                Console.WriteLine(e.Message);
                TempData["msg"] = "Data not found";
                return Page();
           }

        }
        public async Task<IActionResult> OnPostEdit(int id){
            try
            {
                userdetail = await _context.UserDetails.FirstOrDefaultAsync(i=>i.Accounts_Id==id);
                if(userdetail!=null){
                    userdetail.UserName = udmc.UserName;
                    userdetail.Area = udmc.Area;
                    userdetail.Landmark = udmc.Landmark;
                    userdetail.Pincode = udmc.Pincode;
                    await _context.SaveChangesAsync();
                    return RedirectToPage("../UserProfile/UsersProfile");
                }
                var usrdet = new UserDetails{
                    Accounts_Id = id,
                    UserName = udmc.UserName,
                    Area = udmc.Area,
                    Landmark = udmc.Landmark,
                    Pincode = udmc.Pincode,
                };
                await _context.UserDetails.AddAsync(usrdet);
                await _context.SaveChangesAsync();
                return RedirectToPage("../UserProfile/UsersProfile");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Page();
            }
        }
    }
}