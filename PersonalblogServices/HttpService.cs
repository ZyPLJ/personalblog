using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Personalblog.Model.ViewModels;

namespace PersonalblogServices;
public interface IHttpService
{
    Task<string> SendGetRequest(HttpSend httpSend);
    Task<string> SendPostRequest(HttpSend httpSend);
}

public class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;

    public HttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> SendGetRequest(HttpSend httpSend)
    {
        string requestUrl = $"{httpSend.Url}/{httpSend.Title}/{httpSend.Content}";
        HttpResponseMessage response = await _httpClient.GetAsync(requestUrl);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        throw new HttpRequestException($"请求失败：{response.StatusCode}");
    }

    public async Task<string> SendPostRequest(HttpSend httpSend)
    {
        var httpContent = new StringContent(httpSend.Content, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _httpClient.PostAsync(httpSend.Url, httpContent);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        throw new HttpRequestException($"请求失败：{response.StatusCode}");
    }
}
