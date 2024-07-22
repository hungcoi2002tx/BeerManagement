using Client.Pages.UserInfo;
using Client.WebRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models;
using Share.Models.Domain;
using Share.Ultils;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Client.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IHttpContextAccessor httpContextAccessor;

        [BindProperty]
        public UserLogin User { get; set; }

        public LoginModel(ICustomHttpClient request, IHttpContextAccessor httpContextAccessor)
        {
            _request = request;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if(token != null)
            {
                return Redirect(GlobalVariants.PAGE_503);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || User == null)
            {
                return Page();
            }
            var loginModel = new UserLogin
            {
                UserName = User.UserName,
                Password = User.Password
            };

            var response = await _request.PostJsonAsync("https://localhost:7169/api/Login", loginModel);
            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ModelState.AddModelError("","Wrong Username or Password");
                return Page();
            }
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();

                HttpContext.Session.SetString("JWToken", responseBody);
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if(identity != null)
                {
                    var userClaim = identity.Claims;
                    HttpContext.Session.SetString("UserName", User.UserName);
                }
            }
            else
            {
                // Đọc chi tiết lỗi từ API để chẩn đoán
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {response.StatusCode}, Details: {errorContent}");
                return null;
            }

            return Redirect("/Dashboard/Index");
        }

        public async Task<IActionResult> OnGetABCAsync()
        {
            var response = await _request.GetAsync("https://localhost:7169/api/Category");

            if (response.IsSuccessStatusCode)
            {
                //var list = await response.Content.ReadFromJsonAsync<List<Category>>();
                return RedirectToPage();
            }
            else
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToPage();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    Console.WriteLine("Error: Internal Server Error (500). Something went wrong on the server.");
                    return RedirectToPage();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Details: {errorContent}");
                    return RedirectToPage();
                }
            }
        }
    }
    public class TokenResponse
    {
        public string Token { get; set; }
    }
}
