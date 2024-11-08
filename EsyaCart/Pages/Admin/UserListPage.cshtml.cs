using EsyaCart.Data.Context;
using EsyaCart.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EsyaCart.Pages.Admin
{
    public class UserListPageModel : PageModel
    {
        private readonly AppDbContext _context;


        public List<UserDetails> UserList { get; set; }

        public List<AllUserDetails> AllUserDetailsList { get; set; }

        public UserListPageModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task OnGet()
        {
            AllUserDetailsList = await (from Account in _context.Account
                                        join UserDetails in _context.UserDetails
                                        on Account.Account_Id equals UserDetails.Accounts_Id
                                        where Account.Account_type == "USER" 
                                        select new AllUserDetails
                                        {
                                            UserDetails_Id = UserDetails.UserDetails_Id,
                                            UserName = UserDetails.UserName,
                                            Area = UserDetails.Area,
                                            Landmark = UserDetails.Landmark,
                                            Pincode = UserDetails.Pincode,
                                            CreatedAt = Account.CreatedAt,
                                            IsActive = Account.IsActive,
                                        }).ToListAsync();

            /*UserList = _context.UserDetails.ToList();*/
        }
    }
    public class AllUserDetails
    {
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }

        public string Account_type { get; set; }

        public int UserDetails_Id { get; set; }

        public string UserName { get; set; }

        public string Area { get; set; }

        public string Landmark { get; set; }

        public int Pincode { get; set; }

    }
}
