using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Personalblog.Contrib.SiteMessage;
using Personalblog.Extensions.SendEmail;
using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.LinkExchange;
using PersonalblogServices.Links;
using Messages = Personalblog.Contrib.SiteMessage.Messages;

namespace Personalblog.Controllers;

public class LinkExchangeController : Controller
{
    private readonly ILinkExchangeService _linkExchangeService;
    private readonly Messages _messages;
    private readonly IMapper _mapper;
    private readonly ILinkService _linkService;

    public LinkExchangeController(ILinkExchangeService linkExchangeService,Messages messages,
        IMapper mapper,ILinkService linkService)
    {
        _linkExchangeService = linkExchangeService;
        _linkService = linkService;
        _messages = messages;
        _mapper = mapper;
    }
    // GET
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var links = await _linkService.GetAll();
        ViewData["Links"] = links;
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Add(LinkExchangeAddViewModel vm) {
        var links = await _linkService.GetAll();
        ViewData["Links"] = links;
        if (!ModelState.IsValid) return View();

        if (await _linkExchangeService.HasUrl(vm.Url)) {
            _messages.Error("相同网址的友链申请已提交！");
            return View();
        }

        var item = _mapper.Map<LinkExchange>(vm);
        item = await _linkExchangeService.AddOrUpdate(item);
        
        await _linkExchangeService.SendEmailOnAdd(item);

        _messages.Info("友链申请已提交，正在处理中，请及时关注邮件通知~");
        return RedirectToAction("Index", "Home");
    }
}