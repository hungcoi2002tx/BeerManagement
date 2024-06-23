using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Models;
using Share.Models.Domain;
using Share.Ultils;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Client.Pages
{
    public class LoginModel : PageModel
    {
        private HttpClient _httpClient;
        private readonly CustomHttpClient _customHttpClient;

        [BindProperty]
        public UserLogin User { get; set; }

        public LoginModel(HttpClient httpClient, CustomHttpClient customHttpClient)
        {
            _httpClient = httpClient;
            _customHttpClient = customHttpClient;
        }

        public void OnGet()
        {
        }

        public async Task<RedirectToPageResult> OnPostAsync()
        {
            var loginModel = new UserLogin
            {
                UserName = User.UserName,
                Password = User.Password
            };
          
            var response = await _customHttpClient.PostJsonAsync("https://localhost:7169/api/Login", loginModel);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                HttpContext.Session.SetString("JWToken", responseBody);
            }
            else
            {
                // Đọc chi tiết lỗi từ API để chẩn đoán
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Details: {errorContent}");
                return null;
            }

            return RedirectToPage(new { handler = "ABC" });
        }

        public async Task OnGetABCAsync()
        {
            var response = await _customHttpClient.GetAsync("https://localhost:7169/api/Category");

            if (response.IsSuccessStatusCode)
            {
                var list = await response.Content.ReadFromJsonAsync<List<Category>>();
            }
        }
    }
    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
