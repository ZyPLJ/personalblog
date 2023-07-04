using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Services;

namespace Personalblog.Controllers;


[ApiExplorerSettings(IgnoreApi = true)]
/*
 * [ApiExplorerSettings(IgnoreApi = true)]
 * 用于标记一个API方法或控制器类，指示API文档生成器忽略该API。当一个API方法或控制器类被标记为 [ApiExplorerSettings(IgnoreApi = true)]，它将不会出现在生成的API文档中，也不会被包含在API的元数据中。
 */
public class RssController : Controller
{
    private readonly MyDbContext _dbContext;
    private readonly ConfigService _conf;

    public RssController(MyDbContext dbContext,ConfigService conf)
    {
        _dbContext = dbContext;
        _conf = conf;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var feedUrl = $"{_conf["host"]}feed";
        ViewBag.FeedUrl = feedUrl;
        return View();
    }
    [ResponseCache(Duration = 1200)]
    [HttpGet("feed")]
    public async Task<IActionResult> Feed()
    {
        var host = _conf["host"];
        var items = new List<SyndicationItem>();
        var posts = await _dbContext.posts.Where(a => a.CreationTime.Year == DateTime.Now.Year)
            .Include(a => a.Categories)
            .ToListAsync();
        var feed = new SyndicationFeed("ZY知识库", "这是一个用于分享知识和经验的平台，我会在这里分享一些我学习和工作中的经验和心得，希望能够对你有所帮助。",
            new Uri($"{host}"), "RSSUrl", posts.First().LastUpdateTime)
        {
            Copyright = new TextSyndicationContent($"{DateTime.Now.Year} ZY知识库")
        };
        foreach (var item in posts)
        {
            var postUrl = Url.Action("Post", "Blog", new { id = $"{item.Id}" }, HttpContext.Request.Scheme);
            items.Add(new SyndicationItem(item.Title,
                new TextSyndicationContent(PostService.GetContentHtml(item),TextSyndicationContentKind.Html),
                new Uri(postUrl),item.Id,item.LastUpdateTime
                )
            {
                Categories = { new SyndicationCategory(item.Categories?.Name) },
                Authors = { new SyndicationPerson("1767992919@qq.com","ZY知识库",$"{_conf["host"]}") },
                PublishDate = item.CreationTime,
                Summary = new TextSyndicationContent(item.Summary)
            });
        }
        
        feed.Items = items;
            
        var settings = new XmlWriterSettings {
            Async = true,
            Encoding = Encoding.UTF8,
            NewLineHandling = NewLineHandling.Entitize,
            NewLineOnAttributes = true,
            Indent = true
        };
        using var stream = new MemoryStream();
        await using var xmlWriter = XmlWriter.Create(stream, settings);
        var rssFormatter = new Atom10FeedFormatter(feed);
        rssFormatter.WriteTo(xmlWriter);
        await xmlWriter.FlushAsync();

        return File(stream.ToArray(), "application/xml; charset=utf-8");
    }
}