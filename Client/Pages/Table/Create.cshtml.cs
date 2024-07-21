using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Ultils;

namespace Client.Pages.Table
{
    public class IndexModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;

        public IndexModel(ICustomHttpClient httpCustom, Logger logger)
        {
            _httpCustom = httpCustom;
            _logger = logger;
        }

        public void OnGet()
        {


        }
    }
}
