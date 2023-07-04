using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.LinkExchange;
using PersonalblogServices.Links;
using PersonalblogServices.Response;

namespace Personalblog.Apis;

/// <summary>
/// 友情链接申请
/// </summary>
[Authorize]
[ApiController]
[Route("Api/[controller]")]
public class LinkExchangeController : ControllerBase
{
    private readonly ILinkExchangeService _linkExchangeService;
    private readonly IMapper _mapper;

    public LinkExchangeController(ILinkExchangeService linkExchangeService,IMapper mapper)
    {
        _linkExchangeService = linkExchangeService;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<List<LinkExchange>> GetAll() {
        return await _linkExchangeService.GetAll();
    }
    [HttpGet("{id:int}")]
    public async Task<ApiResponse<LinkExchange>> Get(int id) {
        var item = await _linkExchangeService.GetById(id);
        return item == null ? ApiResponse.NotFound() : new ApiResponse<LinkExchange>(item);
    }
    [HttpPost("{id:int}/[action]")]
    public async Task<ApiResponse> Accept(int id, [FromBody] LinkExchangeVerityDto dto) {
        if (!await _linkExchangeService.HasId(id)) return ApiResponse.NotFound();
        await _linkExchangeService.SetVerifyStatus(id, true, dto.Reason);
        return ApiResponse.Ok();
    }
    [HttpPost("{id:int}/[action]")]
    public async Task<ApiResponse> Reject(int id, [FromBody] LinkExchangeVerityDto dto) {
        if (!await _linkExchangeService.HasId(id)) return ApiResponse.NotFound();
        await _linkExchangeService.SetVerifyStatus(id, false, dto.Reason);
        return ApiResponse.Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<ApiResponse> Delete(int id) {
        if (!await _linkExchangeService.HasId(id)) return ApiResponse.NotFound();
        var rows = await _linkExchangeService.DeleteById(id);
        return ApiResponse.Ok($"deleted {rows} rows.");
    }
}