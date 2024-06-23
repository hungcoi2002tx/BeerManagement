using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Client.Pages
{
    public class LoginModel : PageModel
    {

        private HttpClient _httpClient;


        [BindProperty]
        public UserLogin User { get; set; }

        public LoginModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public void OnGet()
        {
        }

        public async Task<RedirectToPageResult> OnPostAsync()
        {
            var ac = JsonSerializer.Serialize(User);
            var loginModel = new { UserName = User.UserName, Password = User.Password };
            var content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7169/api/Login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var a = 0;
            }
            else
            {
                // Đọc chi tiết lỗi từ API để chẩn đoán
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Details: {errorContent}");
                return null;
            }
            return RedirectToPage(new { handler = "OnGet" });
        }
    }
    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
