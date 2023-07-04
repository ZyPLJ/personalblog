using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Personalblog.Model.Entitys;

public class Notice
{
    [Key] //主键
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//自增
    public int Id { get; set; }
    public string Content { get; set; }
}