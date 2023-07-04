using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personalblog.Contrib.SiteMessage;
using Personalblog.Model;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Arc;
using PersonalblogServices.ArticelArc;
using X.PagedList;

namespace Personalblog.Controllers;

public class ArticleArcController : Controller
{
    private readonly IArcService _arcService;
    private readonly MyDbContext _myDbContext;
    private readonly Messages _messages;

    public ArticleArcController(IArcService arcService,Messages messages,MyDbContext myDbContext)
    {
        _arcService = arcService;
        _messages = messages;
        _myDbContext = myDbContext;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var data = await _arcService.GetViewPostAsync();
        return View(data);
    }
    public async Task<IActionResult> GetPageData(int page = 1, int pageSize = 2)
    {
        IPagedList<ArcPost> data = await _arcService.GetAllAsync(new QueryParameters
        {
            Page = page,
            PageSize = pageSize,
        });
        if (data.Count == 0) {
            // 没有更多数据了，返回错误
            return NotFound();
        }
        
        return PartialView("Widgets/ArcBox", data);
    }
}