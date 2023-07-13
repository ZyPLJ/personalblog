namespace Personalblog.Model.ViewModels.Blog;

public class CommentSendEmailDto
{
    public string name { get; set; }
    public string email { get; set; }
    public string content{ get; set; }
    public string postId{ get; set; }
}