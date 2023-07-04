using PersonalblogServices.Response;

namespace PersonalblogServices.Notice;

public interface INoticeService
{
    Task<List<Personalblog.Model.Entitys.Notice>> GetAllAsync();

    Task<ApiResponse> AddNoticeAsync(string Content);
    //删除
    Task<ApiResponse> DelNoticeAsync(int id);
}