namespace Personalblog.Model.ViewModels.Categories;

public class CategoryCreationDto
{
    public string Name { get; set; }
    public int ParentId { get; set; }
    public bool Visible { get; set; } = true;
}