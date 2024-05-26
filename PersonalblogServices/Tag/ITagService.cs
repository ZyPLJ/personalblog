using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels;
using Personalblog.Model.ViewModels.Categories;
using Personalblog.Model.ViewModels.QueryFilters;
using Personalblog.Model.ViewModels.Tag;
using PersonalblogServices.Response;

namespace PersonalblogServices.Tag;

public interface ITagService
{
    /// <summary>
    /// 查询所有标签
    /// </summary>
    /// <returns></returns>
    Task<List<Personalblog.Model.Entitys.Tag>> GetAllTagAsync();

    Task<(List<PostTag>, PaginationMetadata)> GetAllTagPostAsync(QueryParameters param);
    /// <summary>
    /// 根据id去查询标签
    /// </summary>
    /// <param name="tagId"></param>
    /// <returns></returns>
    Task<Personalblog.Model.Entitys.Tag?> GetById(int tagId);

    /// <summary>
    /// 新增标签
    /// </summary>
    /// <param name="tagName"></param>
    /// <returns></returns>
    Task<ApiResponse> AddTagAsync(TagCreateDto dto);
    /// <summary>
    /// 单个删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ApiResponse> DelTagAsync(int id);
    /// <summary>
    /// 编辑标签
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ApiResponse> UpdateTagAsync(TagUpdateDto tagUpdateDto);
    /// <summary>
    /// 分页查询
    /// </summary>
    /// <param name="queryParameter"></param>
    /// <returns></returns>

    Task<(List<TagDto>, PaginationMetadata)> GetPagedList(QueryParameters queryParameter);
}