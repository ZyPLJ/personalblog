using Personalblog.Model.Entitys;

namespace Personalblog.Model.ViewModels.Arc;

public class ArcPost
{
    public int Year { get; set; }
    public int Month { get; set; }
    public List<Post> Posts { get; set; }
}