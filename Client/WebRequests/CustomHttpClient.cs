using System.Text.Json;
using System.Text;

namespace Client.WebRequests
{
    public class CustomHttpClient : ICustomHttpClient
    {

        private readonly HttpClient _httpClient;

        public CustomHttpClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            //var token = httpContextAccessor.HttpContext.Session.GetString("JWToken");
            //_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string requestUri)
        {
            return await _httpClient.DeleteAsync(requestUri);
        }

        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _httpClient.GetAsync(requestUri);
        }

        public async Task<T> GetFromJsonAsync<T>(string requestUri)
        {
            return await _httpClient.GetFromJsonAsync<T>(requestUri);
        }

        public async Task<HttpResponseMessage> PostJsonAsync<T>(string requestUri, T obj)
        {
            var content = new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(requestUri, content);
        }
    }
}
