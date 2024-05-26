using Microsoft.AspNetCore.Mvc;
using Personalblog.Contrib.SiteMessage;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Tag;
using PersonalblogServices.Tag;

namespace Personalblog.Controllers;

public class TagController : Controller
{
    private readonly ITagService _tagService;
    private readonly Messages _messages;

    public TagController(ITagService tagService,Messages messages)
    {
        _tagService = tagService;
        _messages = messages;
    }
    // GET
    public async Task<IActionResult> Index(int tagId = 1, int page = 1, int pageSize = 8)
    {
        var TagList = await _tagService.GetAllTagAsync();
        if (TagList.Count == 0)
            return View(new TagIndex()
            {
                TagList = await _tagService.GetAllTagAsync(),
                TagId = 0,
                TagAllPosts = await _tagService.GetAllTagPostAsync(
                    new QueryParameters
                    {
                        Page = page,
                        PageSize = pageSize,
                        TagId = tagId,
                    })
            });
        
        var currentTag = tagId == 1 ? TagList[0] :await _tagService.GetById(tagId);
        if (currentTag == null)
        {
            _messages.Error($"标签 {currentTag} 不存在！");
            return RedirectToAction(nameof(Index));
        }

        TagIndex tagIndex = new TagIndex()
        {
            TagList = await _tagService.GetAllTagAsync(),
            TagId = currentTag.Id,
            TagAllPosts = await _tagService.GetAllTagPostAsync(
                new QueryParameters
                {
                    Page = page,
                    PageSize = pageSize,
                    TagId = currentTag.Id,
                })
        };
        return View(tagIndex);
    }
}