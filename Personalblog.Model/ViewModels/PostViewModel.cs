using Personalblog.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Personalblog.Model.Extensions.Markdown;

namespace Personalblog.Model.ViewModels
{
    public class PostViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string ContentHtml { get; set; }
        public int ViewCount { get; set; }
        public string Path { get; set; }
        public string? Url { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public Category Category { get; set; }
        public List<Category> Categories { get; set; }
        public List<Comment> CommentsList { get; set; }
        public ConfigItem ConfigItem { get; set; }
        public List<TocNode>? TocNodes { get; set; }

        public string TocNodesJson => JsonSerializer.Serialize(
            TocNodes,
            new JsonSerializerOptions {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        );
    }
}
