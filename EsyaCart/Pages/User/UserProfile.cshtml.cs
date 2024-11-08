using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EsyaCart.Pages.User
{
    public class UserProfile : PageModel
    {
        private readonly ILogger<UserProfile> _logger;

        public UserProfile(ILogger<UserProfile> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}