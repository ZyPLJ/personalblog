using Personalblog.Model.Entitys;

namespace PersonalblogServices.Links;

public interface ILinkExchangeService
{
    //查询id是否存在
    Task<bool> HasId(int id);
    Task<bool> HasUrl(string url);
    Task<List<LinkExchange>> GetAll();
    Task<LinkExchange?> GetById(int id);
    Task<LinkExchange> AddOrUpdate(LinkExchange item);
    Task<LinkExchange?> SetVerifyStatus(int id, bool status, string? reason = null);
    Task<int> DeleteById(int id);
    Task SendEmailOnAdd(LinkExchange item);
    Task SendEmailOnAccept(LinkExchange item);
    Task SendEmailOnReject(LinkExchange item);
}