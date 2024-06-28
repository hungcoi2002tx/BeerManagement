using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Pages.Category
{
    public class CategoryModel : PageModel
    {
        private readonly ICustomHttpClient _httpClient;

        public CategoryModel(ICustomHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnGet()
        {
            var response = _httpClient.GetAsync("https://localhost:7169/api/Category");
            if (response != null)
            {
                var body = response.Result;
            }
        }
    }
}
