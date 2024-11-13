using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EsyaCart.Pages.User
{
    public class UserLogout : PageModel
    {
        private readonly ILogger<UserLogout> _logger;

        public UserLogout(ILogger<UserLogout> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("../User/Dashboard");
        }
    }
}