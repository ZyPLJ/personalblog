using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Links;
using PersonalblogServices.Links;
using PersonalblogServices.Response;
using StackExchange.Profiling.Data;

namespace Personalblog.Apis;

/// <summary>
/// 友情链接
/// </summary>
[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class LinkController : ControllerBase
{
    private readonly ILinkService _linkService;
    private readonly IMapper _mapper;

    public LinkController(ILinkService linkService, IMapper mapper)
    {
        _linkService = linkService;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<List<Link>> GetAll() {
        return await _linkService.GetAll(false);
    }

    [HttpGet("{id:int}")]
    public async Task<ApiResponse<Link>> Get(int id) {
        var item = await _linkService.GetById(id);
        return item == null ? ApiResponse.NotFound() : new ApiResponse<Link>(item);
    }

    [HttpPost]
    public async Task<Link> Add(LinkCreationDto dto) {
        var link = _mapper.Map<Link>(dto);
        link = await _linkService.AddOrUpdate(link);
        return link;
    }

    [HttpPut("{id:int}")]
    public async Task<ApiResponse<Link>> Update(int id, LinkCreationDto dto) {
        var item = await _linkService.GetById(id);
        if (item == null) return ApiResponse.NotFound();

        var link = _mapper.Map(dto, item);
        link = await _linkService.AddOrUpdate(link);
        return new ApiResponse<Link>(link);
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse> Delete(int id) {
        if (!await _linkService.HasId(id)) return ApiResponse.NotFound();
        var rows = await _linkService.DeleteById(id);
        return ApiResponse.Ok($"deleted {rows} rows.");
    }
}