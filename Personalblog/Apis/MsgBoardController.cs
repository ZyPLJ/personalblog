using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.QueryFilters;
using PersonalblogServices.Messages;
using PersonalblogServices.Response;

namespace Personalblog.Apis;

[ApiController]
[Route("Api/[controller]")]
public class MsgBoardController : ControllerBase
{
    private readonly IMessagesService _messagesService;

    public MsgBoardController(IMessagesService messagesService)
    {
        _messagesService = messagesService;
    }

    [HttpGet]
    public ApiResponsePaged<Messages> GetMessage([FromQuery] MsgBoardQueryParameter param)
    {
        var pageList = _messagesService.GetAll(param);
        return new ApiResponsePaged<Messages>(pageList);
    }
    [HttpGet]
    [Route("[action]")]
    public ApiResponsePaged<Replies> GetMessageReply([FromQuery] MsgBoardQueryParameter param)
    {
        var pageList = _messagesService.GetAllReply(param);
        return new ApiResponsePaged<Replies>(pageList);
    }
    [Authorize]
    [HttpDelete("{id:int}")]
    public async Task<ApiResponse> DelMessage(int id)
    {
        return await _messagesService.DelMessageAsync(id);
    }
    [Authorize]
    [HttpDelete("DelMessageReply/{id:int}")]
    public async Task<ApiResponse> DelMessageReply(int id)
    {
        return await _messagesService.DelMessageReplyAsync(id);
    }
}