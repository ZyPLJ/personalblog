using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Arc;
using X.PagedList;

namespace PersonalblogServices.ArticelArc;

public interface IArcService
{
    Task<IPagedList<ArcPost>> GetAllAsync(QueryParameters param);
    Task<ArcViewPost> GetViewPostAsync();
}