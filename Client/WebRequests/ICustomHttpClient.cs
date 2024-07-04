namespace Client.WebRequests
{
    public interface ICustomHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
        Task<T> GetFromJsonAsync<T>(string requestUri);
        Task<HttpResponseMessage> PostJsonAsync<T>(string requestUri, T obj);
        Task<HttpResponseMessage> DeleteAsync(string requestUri);
    }
}