namespace Personalblog.Model.ViewModels;

public class ModelBase
{
    public DateTime CreatedTime { get; set; } = DateTime.Now;
    public DateTime UpdatedTime { get; set; } = DateTime.Now;
}