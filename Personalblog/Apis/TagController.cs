using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.QueryFilters;
using Personalblog.Model.ViewModels.Tag;
using PersonalblogServices.Response;
using PersonalblogServices.Tag;
using Tag = Personalblog.Model.Entitys.Tag;

namespace Personalblog.Apis;

/// <summary>
/// 标签
/// </summary>
[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }
    [HttpGet("All")]
    public async Task<ApiResponse<List<Tag>>> GetAllTask()
    {
        return new ApiResponse<List<Tag>>(await _tagService.GetAllTagAsync());
    }

    [HttpGet("GetPage")]
    public async Task<ApiResponsePaged<TagDto>> GetPageList([FromQuery] QueryParameters @queryParameter)
    {
        var (data, meta) = await _tagService.GetPagedList(@queryParameter);
        return new ApiResponsePaged<TagDto>(data,meta);
    }

    [HttpPost]
    public async Task<ApiResponse> AddTagAsync([FromBody]TagCreateDto tagCreateDto)
    {
        try
        {
            return await _tagService.AddTagAsync(tagCreateDto);
        }
        catch (Exception ex)
        {
            // 处理异常
            return new ApiResponse { Data = null, Message = "发生错误：" + ex.Message, StatusCode = 500 };
        }
    }

    [HttpPut]
    public async Task<ApiResponse> UpdateTagAsync([FromBody]TagUpdateDto tagUpdateDto)
    {
        try
        {
            return await _tagService.UpdateTagAsync(tagUpdateDto);
        }
        catch (Exception ex)
        {
            return new ApiResponse { Data = null, Message = "发生错误：" + ex.Message, StatusCode = 500 };
        }
    }
    /// <summary>
    /// 单个删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ApiResponse> DelTag(int id)
    {
        return await _tagService.DelTagAsync(id);
    }
}