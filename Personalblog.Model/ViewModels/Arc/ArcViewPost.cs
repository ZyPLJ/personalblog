namespace Personalblog.Model.ViewModels.Arc;

public class ArcViewPost
{
    /// <summary>
    /// 阅读量
    /// </summary>
    public int ViewCount { get; set; }
    /// <summary>
    /// 评论数
    /// </summary>
    public int CommentCount { get; set; }
    /// <summary>
    /// 文章数
    /// </summary>
    public int PostCount { get; set; }
    /// <summary>
    /// 分类数
    /// </summary>
    public int CateGoryCount { get; set; }
}