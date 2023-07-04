using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personalblog.Model.Entitys;

public class Replies
{
    [Key] //主键
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增
    public int Id { get; set; }
    public int MessageId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Reply { get; set; }
    public DateTime created_at { get; set; }
    public virtual Messages? Message { get; set; }
}