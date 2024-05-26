using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Extensions;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Blog;
using Personalblog.Model.ViewModels.QueryFilters;
using Personalblog.Services;
using PersonalblogServices.Articels;
using PersonalblogServices.CommentService;
using PersonalblogServices.Response;

namespace Personalblog.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly Icommentservice _commentservice;
    private readonly IArticelsService _articelsService;
    private readonly TempFilterService _filter;

    public CommentController(Icommentservice commentservice,
        IArticelsService articelsService,
        TempFilterService filter)
    {
        _commentservice = commentservice;
        _articelsService = articelsService;
        _filter = filter;
    }

    /// <summary>
    /// 获取分页评论 可查询全部 可根据文章id 评论内容模糊查询
    /// </summary>
    /// <param name="params"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ApiResponsePaged<Comment>> GetPagedList([FromQuery] CommentQueryParameter @params)
    {
        var (data, meta) = await _commentservice.GetPagedList(@params);
        return new ApiResponsePaged<Comment>(data, meta);
    }

    [HttpPost]
    public async Task<ApiResponse<Comment>> Add(CommentCreationDto dto)
    {
        var anonymousUser = await _commentservice.GetOrCreateAnonymousUser(
            dto.UserName, dto.Email, dto.Url,
            HttpContext.GetRemoteIPAddress()?.ToString().Split(":")?.Last()
        );
        var comment = new Comment()
        {
            ParentId = dto.ParentId,
            PostId = dto.PostId,
            AnonymousUserId = anonymousUser.Id,
            UserAgent = Request.Headers.UserAgent,
            Content = dto.Content
        };
        string msg;
        if (_filter.CheckBadWord(dto.Content))
        {
            msg = "小管家发现您可能使用了不良用语，该评论将在审核通过后展示~";
        }
        else
        {
            msg = "评论由小管家审核通过，感谢您参与讨论~";
            comment = await _commentservice.Add(comment);
        }

        return new ApiResponse<Comment>(comment)
        {
            Message = msg
        };
    }

    [HttpPost]
    [Route("[action]")]
    public ApiResponse Send(CommentSendEmailDto emailDto)
    {
        Task.Run(async () => { await _articelsService.SendEmailOnAdd(emailDto); });
        return new ApiResponse() { StatusCode = 200, Successful = true, Message = "邮件发送成功！" };
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ApiResponse> DelComment(string id)
    {
        return await _commentservice.DelCommentAsync(id);
    }

    [Authorize]
    [HttpDelete("[action]")]
    public async Task<ApiResponse> RangeDelComment([FromBody] List<string> id)
    {
        return await _commentservice.RangeDelAsync(id);
    }
}