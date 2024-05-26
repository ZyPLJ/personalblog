using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personalblog.Model.Entitys;

public class PostTag
{
    /// <summary>
    /// 文章ID
    /// </summary>
    public string PostId { get; set; }

    /// <summary>
    /// 标签ID
    /// </summary>
    public int TagId { get; set; }

    /// <summary>
    /// 导航属性 - 文章
    /// </summary>
    public Post Post { get; set; } = null!;

    /// <summary>
    /// 导航属性 - 标签
    /// </summary>
    public Tag Tag { get; set; } = null!;
}