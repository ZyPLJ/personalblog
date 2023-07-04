namespace Personalblog.Model.ViewModels
{
    public class CategoryNode
    {
        public string? text { get; set; }
        public string? href { get; set; }
        public List<string> tags { get; set; }
        public List<CategoryNode>? nodes { get; set; }
    }
}
