using EsyaCart.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EsyaCart.Pages.Admin
{
    public class AdminHomePageModel : PageModel
    {
        public readonly AppDbContext _context;

        public AdminHomePageModel(AppDbContext context) { _context = context; }
        public int UserCount { get; set; }
        public int VenderCount {  get; set; }

        public void OnGet()
        {
            UserCount = _context.UserDetails.Count();
            VenderCount = _context.VendorDetails.Count();
        }
    }
}
