using Personalblog.Model.Entitys;

namespace Personalblog.Model.ViewModels;

public class FeaturedPostTopCardModel
{
    public Post FeaturedPost { get; set; }
    public int Number { get; set; } // 新添加的属性
}