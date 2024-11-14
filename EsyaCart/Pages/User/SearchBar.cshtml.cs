using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace EsyaCart.Pages.User
{
    public class SearchBar : PageModel
    {
        private readonly ILogger<SearchBar> _logger;

        public SearchBar(ILogger<SearchBar> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}