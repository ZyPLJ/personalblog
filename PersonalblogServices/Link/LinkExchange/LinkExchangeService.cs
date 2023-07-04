using System.Text;
using Microsoft.EntityFrameworkCore;
using Personalblog.Model;
using Personalblog.Model.Entitys;

namespace PersonalblogServices.Links;

public class LinkExchangeService:ILinkExchangeService
{
    private readonly ILinkService _linkService;
    private readonly MyDbContext _myDbContext;

    public LinkExchangeService(ILinkService linkService, MyDbContext myDbContext)
    {
        _myDbContext = myDbContext;
        _linkService = linkService;
    }
    public async Task<bool> HasId(int id)
    {
        return await _myDbContext.LinkExchanges.Where(a => a.Id == id).AnyAsync();
    }
    /// <summary>
    /// 查询 id 是否存在
    /// </summary>
    public async Task<bool> HasUrl(string url)
    {
        return await _myDbContext.LinkExchanges.Where(a => a.Url.Contains(url)).AnyAsync();
    }

    public async Task<List<LinkExchange>> GetAll()
    {
        return await _myDbContext.LinkExchanges.ToListAsync();
    }

    public async Task<LinkExchange?> GetById(int id)
    {
        return await _myDbContext.LinkExchanges.Where(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<LinkExchange> AddOrUpdate(LinkExchange item)
    {
        var data = await _myDbContext.LinkExchanges.FirstOrDefaultAsync(l => l.Id == item.Id);
        if (data != null)
        {
            _myDbContext.LinkExchanges.Update(data);
        }
        else
        {
            await _myDbContext.LinkExchanges.AddAsync(item);
        }
        await _myDbContext.SaveChangesAsync();
        return item;
    }

    public async Task<LinkExchange?> SetVerifyStatus(int id, bool status, string? reason = null)
    {
        var item = await GetById(id);
        item.Verified = status;
        item.Reason = reason;
        _myDbContext.LinkExchanges.Update(item);
        await _myDbContext.SaveChangesAsync();

        var link = await _linkService.GetByName(item.Name);
        if (status)
        {
            await SendEmailOnAccept(item);
            if (link == null)
            {
                await _linkService.AddOrUpdate(new Link
                {
                    Name = item.Name,
                    Description = item.Description,
                    Url = item.Url,
                    Visible = true
                });
            }
            else
            {
                await _linkService.SetVisibility(link.Id, true);
            }
        }
        else
        {
            await SendEmailOnReject(item);
            if (link != null) await _linkService.DeleteById(link.Id);
        }

        return await GetById(id);
    }

    public async Task<int> DeleteById(int id)
    {
        var linkExchange = new LinkExchange { Id = id };
        _myDbContext.LinkExchanges.Attach(linkExchange);
        _myDbContext.LinkExchanges.Remove(linkExchange);
        return await _myDbContext.SaveChangesAsync();
    }

    public async Task SendEmailOnAdd(LinkExchange item)
    {
        const string blogLink = "<a href=\"https://pljzy.top\">ZY知识库</a>";
        var sb = new StringBuilder();
        sb.AppendLine($"<p>友链申请已提交，正在处理中，请及时关注邮件通知~</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>以下是您申请的友链信息：</p>");
        sb.AppendLine($"<p>网站名称：{item.Name}</p>");
        sb.AppendLine($"<p>介绍：{item.Description}</p>");
        sb.AppendLine($"<p>网址：{item.Url}</p>");
        sb.AppendLine($"<p>站长：{item.WebMaster}</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>本消息由 {blogLink} 自动发送，无需回复。</p>");
        await EmailUtils.SendEmailAsync(item.Email, sb.ToString());
    }

    public async Task SendEmailOnAccept(LinkExchange item)
    {
        const string blogLink = "<a href=\"https://pljzy.top\">ZY知识库</a>";
        var sb = new StringBuilder();
        sb.AppendLine($"<p>您好，友链申请已通过！感谢支持，欢迎互访哦~</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>以下是您申请的友链信息：</p>");
        sb.AppendLine($"<p>网站名称：{item.Name}</p>");
        sb.AppendLine($"<p>介绍：{item.Description}</p>");
        sb.AppendLine($"<p>网址：{item.Url}</p>");
        sb.AppendLine($"<p>站长：{item.WebMaster}</p>");
        sb.AppendLine($"<p>补充信息：{item.Reason}</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>本消息由 {blogLink} 自动发送，无需回复。</p>");
        await EmailUtils.SendEmailAsync(item.Email, sb.ToString());
    }

    public async Task SendEmailOnReject(LinkExchange item)
    {
        const string blogLink = "<a href=\"https://pljzy.top\">ZY知识库</a>";
        var sb = new StringBuilder();
        sb.AppendLine($"<p>很抱歉，友链申请未通过！建议您查看补充信息，调整后再次进行申请，感谢您的理解与支持~</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>以下是您申请的友链信息：</p>");
        sb.AppendLine($"<p>网站名称：{item.Name}</p>");
        sb.AppendLine($"<p>介绍：{item.Description}</p>");
        sb.AppendLine($"<p>网址：{item.Url}</p>");
        sb.AppendLine($"<p>站长：{item.WebMaster}</p>");
        sb.AppendLine($"<p>补充信息：{item.Reason}</p>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<br>");
        sb.AppendLine($"<p>本消息由 {blogLink} 自动发送，无需回复。</p>");
        await EmailUtils.SendEmailAsync(item.Email, sb.ToString());
    }
}