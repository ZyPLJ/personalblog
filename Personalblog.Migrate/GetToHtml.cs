namespace Personalblog.Migrate;

public static class GetToHtml
{
    public static string GetFeedbackHtml(string name, string content, DateTime date)
    {
        string html = $@"
    <div class=""feedbackItem"">
        <div class=""feedbackListSubtitle feedbackListSubtitle-louzhu"">
            <span class=""comment_date"">{date}</span>
            <span class=""a_comment_author_5166961"">{name}</span>
        </div>
        <div class=""feedbackCon"">
            {content}
        </div>   
    </div>";
        return html;
    }

}