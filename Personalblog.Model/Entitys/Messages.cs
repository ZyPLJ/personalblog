using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personalblog.Model.Entitys;

/// <summary>
/// 留言表
/// </summary>
public class Messages
{
   [Key] //主键
   [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增
   public int Id { get; set; }
   public string Name { get; set; }
   public string Email { get; set; }
   public string Message { get; set; }
   public DateTime created_at { get; set; }
   public virtual ICollection<Replies>? Replies { get; set; }
}