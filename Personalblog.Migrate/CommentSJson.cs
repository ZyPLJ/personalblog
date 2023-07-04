using System.Text;
using Newtonsoft.Json.Linq;

namespace Personalblog.Migrate;

public static class CommentSJson
{
    public static StringBuilder CommentsJson(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return new StringBuilder();
        }
        JArray jsonArray = JArray.Parse(json);
        StringBuilder htmlBuilder = new StringBuilder();
        foreach (JObject item in jsonArray)
        {
            if (item.ContainsKey("insert"))
            {
                string text = item["insert"].ToString().Replace("\n", "<br>");
                if (item.ContainsKey("attributes"))
                {
                    JObject attributes = JObject.Parse(item["attributes"].ToString());
                    string formattedText = text;
                    if (attributes.ContainsKey("bold") && (bool)attributes["bold"])
                    {
                        formattedText = $"<strong>{formattedText}</strong>";
                    }
                    if (attributes.ContainsKey("italic") && (bool)attributes["italic"])
                    {
                        formattedText = $"<em>{formattedText}</em>";
                    }
                    if (attributes.ContainsKey("underline") && (bool)attributes["underline"])
                    {
                        formattedText = $"<u>{formattedText}</u>";
                    }
                    if (attributes.ContainsKey("strike") && (bool)attributes["underline"])
                    {
                        formattedText = $"<s>{formattedText}</s>";
                    }
                    if (attributes.ContainsKey("link"))
                    {
                        string link = attributes["link"].ToString();
                        htmlBuilder.Append($"<a href=\"{link}\">{formattedText}</a>");
                    }
                    else
                    {
                        htmlBuilder.Append(formattedText);
                    }
                }
                else
                {
                    htmlBuilder.Append(text);
                }
            }
        }
        return htmlBuilder;
    }
}