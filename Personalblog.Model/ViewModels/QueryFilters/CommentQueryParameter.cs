namespace Personalblog.Model.ViewModels.QueryFilters;

public class CommentQueryParameter:QueryParameters
{
    /// <summary>
    /// 排序字段
    /// </summary>
    public new string? Content { get; set; }
    /// <summary>
    /// 文章id
    /// </summary>
    public string? PostId { get; set; }
}