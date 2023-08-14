using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Library.Views.Catalog
{
    public class Hold : PageModel
    {
        private readonly ILogger<Hold> _logger;

        public Hold(ILogger<Hold> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}