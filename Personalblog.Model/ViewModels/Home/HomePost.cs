using Personalblog.Model.Entitys;

namespace Personalblog.Model.ViewModels.Home;

public class HomePost
{
    /// <summary>
    /// 阅读最多
    /// </summary>
    public Post ViewCountMax { get; set; }
    /// <summary>
    /// 评论最多
    /// </summary>
    public Post CommentMax { get; set; }
    public Post FirstPost { get; set; }
    public Post LastPost { get; set; }
}