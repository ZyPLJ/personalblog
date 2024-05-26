using System.ComponentModel.DataAnnotations;

namespace Personalblog.Model.Entitys;

public class Tag
{
    /// <summary>
    /// 标签ID
    /// </summary>
    [Key] //主键
    public int Id { get; set; }

    /// <summary>
    /// 标签名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 导航属性 - 文章列表
    /// </summary>
    public List<Post>? Posts { get; set; }
    public List<PostTag> PostTags { get; } = new();
}