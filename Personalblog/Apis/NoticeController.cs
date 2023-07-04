using Microsoft.AspNetCore.Mvc;
using PersonalblogServices.Notice;
using PersonalblogServices.Response;

namespace Personalblog.Apis;

[ApiController]
[Route("Api/[controller]")]
public class NoticeController : ControllerBase
{
    private readonly INoticeService _noticeService;

    public NoticeController(INoticeService noticeService)
    {
        _noticeService = noticeService;
    }
    // GET
    [HttpGet]
    public async Task<ApiResponse> Add([FromQuery]string Content)
    {
        return await _noticeService.AddNoticeAsync(Content);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<ApiResponse> Get()
    {
        return new ApiResponse(){Data =await _noticeService.GetAllAsync()};
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse> Del(int id)
    {
        return await _noticeService.DelNoticeAsync(id);
    }
}