using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace Personalblog.Apis;

public class GetImagesController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    public GetImagesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    // GET
    public async Task<string> GetImageData(int width,int height)
    {
        // 请求地址
        var apiUrl = $"http://zy.pljzy.top/Api/Image/GetImage?width={width}&height={height}";

        // 创建HttpClient对象
        var client = _httpClientFactory.CreateClient();

        // 发送GET请求
        var response = await client.GetAsync(apiUrl);

        // 确认响应成功
        response.EnsureSuccessStatusCode();

        // 读取响应内容
        var responseContent = await response.Content.ReadAsStringAsync();

        // 解析响应内容
        var responseJson = JsonSerializer.Deserialize<JsonElement>(responseContent);

        // 获取data值
        var imageData = responseJson.GetProperty("data").GetString();

        return imageData;
    }
}