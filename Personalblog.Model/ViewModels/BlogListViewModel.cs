using Personalblog.Model.Entitys;
using System.Text.Encodings.Web;
using System.Text.Json;
using X.PagedList;

namespace Personalblog.Model.ViewModels
{
    public class BlogListViewModel
    {
        public Category CurrentCategory { get; set; }
        public int CurrentCategoryId { get; set; }
        public IPagedList<Post> Posts { get; set; }
        public List<Category> Categories { get; set; }
        public List<CategoryNode>? CategoryNodes { get; set; }

        public string CategoryNodesJson => JsonSerializer.Serialize(
            CategoryNodes,
            new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }
        );
    }
}
