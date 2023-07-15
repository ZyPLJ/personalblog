using System.Text;
using Microsoft.EntityFrameworkCore;
using Personalblog.Extensions.SendEmail;
using Personalblog.Extensions.SendEmail.Services;
using Personalblog.Model;
using Personalblog.Model.Entitys;

namespace PersonalblogServices.Links;

public class LinkExchangeService:ILinkExchangeService
{
    private IEmailService emailService;
    private readonly ILinkService _linkService;
    private readonly MyDbContext _myDbContext;
    private readonly EmailServiceFactory _emailServiceFactory;

    public LinkExchangeService(ILinkService linkService, MyDbContext myDbContext,
        EmailServiceFactory emailServiceFactory)
    {
        _myDbContext = myDbContext;
        _linkService = linkService;
        _emailServiceFactory = emailServiceFactory;
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
                    favicon = item.favicon,
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
        emailService = await _emailServiceFactory.CreateEmailService();
        var template = new LinksNotificationEmailTemplate(); 
        await emailService.SendEmail(item.Email, template,
            new EmailContent<LinkExchange>()
            {
                Content = "您好，友链申请已提交！感谢支持，请留意邮箱信息~",
                Data = item
            });
        // await EmailUtils.SendEmailAsync(item.Email, sb.ToString());
    }

    public async Task SendEmailOnAccept(LinkExchange item)
    {
        emailService = await _emailServiceFactory.CreateEmailService();
        var template = new LinksNotificationEmailTemplate(); 
        await emailService.SendEmail(item.Email, template,
            new EmailContent<LinkExchange>()
            {
                Content = "您好，友链申请已通过！感谢支持，欢迎互访哦~",
                Data = item
            });
    }

    public async Task SendEmailOnReject(LinkExchange item)
    {
        emailService = await _emailServiceFactory.CreateEmailService();
        var template = new LinksNotificationEmailTemplate(); 
        await emailService.SendEmail(item.Email, template,
            new EmailContent<LinkExchange>()
            {
                Content = "很抱歉，友链申请未通过！建议您查看补充信息，调整后再次进行申请，感谢您的理解与支持~",
                Data = item
            });
    }
}