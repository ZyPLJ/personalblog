using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.QueryFilters;
using PersonalblogServices.CommentService;
using PersonalblogServices.Response;

namespace Personalblog.Controllers;

[ApiController]
[Route("Api/[controller]")]
public class CommentController : ControllerBase
{
    private readonly Icommentservice _commentservice;

    public CommentController(Icommentservice commentservice)
    {
        _commentservice = commentservice;
    }
    // GET
    [HttpGet]
    public ApiResponsePaged<Comments> GetComment([FromQuery] CommentQueryParameter param)
    {
        var pageList = _commentservice.GetPagedCommentlist(param);
        return new ApiResponsePaged<Comments>(pageList);
    }
    [Authorize]
    [HttpDelete("{cid:int}")]
    public async Task<ApiResponse> DelComent(int cid)
    {
        return await _commentservice.DelCommentAsync(cid);
    }
}