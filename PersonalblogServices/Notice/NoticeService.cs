using Personalblog.Model;
using PersonalblogServices.Response;
using X.PagedList;

namespace PersonalblogServices.Notice;

public class NoticeService:INoticeService
{
    private readonly MyDbContext _dbContext;

    public NoticeService(MyDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Personalblog.Model.Entitys.Notice>>  GetAllAsync()
    {
        return await _dbContext.notice.ToListAsync();
    }

    public async Task<ApiResponse> AddNoticeAsync(string Content)
    {
        try
        {
            await _dbContext.notice.AddAsync(new Personalblog.Model.Entitys.Notice(){Content = Content});
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return new ApiResponse() { Message = $"{e.Message}" };
        }
        return new ApiResponse() { Message = $"添加成功" };
    }

    public async Task<ApiResponse> DelNoticeAsync(int id)
    {
        try
        {
            var Notice = new Personalblog.Model.Entitys.Notice { Id = id };
            _dbContext.notice.Attach(Notice);
            _dbContext.notice.Remove(Notice);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return new ApiResponse() { Message = $"{e.Message}" ,StatusCode = 500};
        }

        return new ApiResponse() { Message = "删除成功" };
    }
}