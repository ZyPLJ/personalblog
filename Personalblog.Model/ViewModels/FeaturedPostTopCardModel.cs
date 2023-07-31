using Personalblog.Model.Entitys;

namespace Personalblog.Model.ViewModels;

public class FeaturedPostTopCardModel
{
    public Post HomePost { get; set; }
    /// <summary>
    /// 1 阅读最多
    /// 2 评论最多
    /// 3 最旧
    /// 4 最新
    /// </summary>
    public int Number { get; set; } // 新添加的属性
}