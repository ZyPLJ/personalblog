using Personalblog.Model.Entitys;
using Personalblog.Model.ViewModels.Categories;

namespace Personalblog.Model.ViewModels.Tag;

public class TagIndex
{
    public List<Entitys.Tag> TagList { get; set; }
    public (List<PostTag>, PaginationMetadata) TagAllPosts { get; set; }
    public int TagId { get; set; }
}