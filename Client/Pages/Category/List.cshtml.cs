using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;

namespace Client.Pages.Category
{
    public class CategoryModel : PageModel
    {
        private readonly ICustomHttpClient _httpClient;

        public CategoryModel(ICustomHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task OnGet()
        {
            var response = await _httpClient.GetAsync(RestApiName.GET_LIST_CATEGORY);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
            }
        }
    }
}
