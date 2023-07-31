using System.Text.RegularExpressions;
using Markdig.Syntax;
using Personalblog.Model.Entitys;

namespace Personalblog.Model.Extensions.Markdown;

class Heading {
    public int Id { get; set; }
    public int Pid { get; set; } = -1;
    public string? Text { get; set; }
    public string? Slug { get; set; }
    public int Level { get; set; }
}

public class TocNode {
    public string? Text { get; set; }
    public string? Href { get; set; }
    public List<string>? Tags { get; set; }
    public List<TocNode>? Nodes { get; set; }
}

public static class ToC {
    public static List<TocNode>? ExtractToc(this MarkdownDocument document) {
        var headings = new List<Heading>();

        foreach (var heading in document.Descendants<HeadingBlock>()) {
            var item = new Heading {Level = heading.Level, Text = heading.Inline?.FirstChild?.ToString()};
            headings.Add(item);
        }

        var chineseTitleCount = 0;
        var slugMap = new Dictionary<string, int>();
        for (var i = 0; i < headings.Count; i++) {
            var item = headings[i];
            item.Id = i;
            if(i==7){}
            var text = item.Text ?? "";
            // 包含中文且不包含英文的转换为 section-1 格式
            if (Regex.IsMatch(text, "^((?![a-zA-Z]).)*[\u4e00-\u9fbb]((?![a-zA-Z]).)*$")) {
                item.Slug = chineseTitleCount == 0 ? "section" : $"section-{chineseTitleCount}";
                chineseTitleCount++;
            }
            // 其他情况处理为只包含英文数字格式
            else {
                item.Slug = Regex.Replace(text, @"[^a-zA-Z0-9\s]+", "")
                    .Trim().Replace(" ", "-").ToLower();
                if (Char.IsDigit(item.Slug[0])) //第一个字符为数字则删除 因为id不支持数字开头
                {
                    item.Slug = item.Slug.Substring(1);
                }
                if (slugMap.ContainsKey(item.Slug)) {
                    item.Slug = $"{item.Slug}-{slugMap[item.Slug]++}";
                }
                else {
                    slugMap[item.Slug] = 1;
                }
            }

            for (var j = i; j >= 0; j--) {
                var preItem = headings[j];
                if (item.Level == preItem.Level + 1) {
                    item.Pid = j;
                    break;
                }
            }
        }

        List<TocNode>? GetNodes(int pid = -1) {
            var nodes = headings.Where(a => a.Pid == pid).ToList();
            return nodes.Count == 0
                ? null
                : nodes.Select(a => new TocNode {Text = a.Text, Href = $"#{a.Slug}", Nodes = GetNodes(a.Id)}).ToList();
        }

        return GetNodes();
        //
        // var headings = new List<Heading>();
        //
        // foreach (var heading in document.Descendants<HeadingBlock>()) {
        //     var item = new Heading {Level = heading.Level, Text = heading.Inline?.FirstChild?.ToString()};
        //     headings.Add(item);
        // }
        //
        // var slugCounts = new Dictionary<string, int>();
        // int chineseTitleCount = 0;
        // for (var i = 0; i < headings.Count; i++) {
        //     var item = headings[i];
        //     item.Id = i;
        //
        //     var text = item.Text ?? "";
        //     if (Regex.IsMatch(text, "[\u4e00-\u9fbb]") && !Regex.IsMatch(text, "[A-Za-z]+")) {
        //         item.Slug = chineseTitleCount == 0 ? "section" : $"section-{chineseTitleCount}";
        //         chineseTitleCount++;
        //     }
        //     else {
        //         // 使用正则表达式匹配英文字符和空格
        //         var regex = new Regex(@"[A-Za-z\s]+");
        //         var matches = regex.Matches(text);
        //         var filteredText = string.Concat(matches);
        //
        //         // 将连续的空格替换为单个空格，并将结果转换为小写
        //         filteredText = Regex.Replace(filteredText, @"\s+", " ").ToLower();
        //
        //         // 移除开头和结尾的空格
        //         filteredText = filteredText.Trim();
        //
        //         // 将剩余的空格替换为连字符（-）
        //         filteredText = filteredText.Replace(" ", "-");
        //
        //         item.Slug = filteredText;
        //     }
        //
        //     // 更新Pid以考虑子目录
        //     for (var j = i; j >= 0; j--) {
        //         var preItem = headings[j];
        //         if (item.Level == preItem.Level + 1) {
        //             item.Pid = j;
        //             // if (item.Slug.StartsWith(preItem.Slug)) {
        //             //     item.Slug = $"{preItem.Slug}-{slugCounts[preItem.Slug]}";
        //             // }
        //             break;
        //         }
        //     }
        //
        //
        //     // 检查并处理同名目录
        //     if (slugCounts.ContainsKey(item.Slug)) {
        //         slugCounts[item.Slug]++;
        //     } else {
        //         slugCounts[item.Slug] = 1;
        //     }
        //
        //     // 为同级同名目录添加计数，从1开始，第一个目录不需要编号
        //     if (slugCounts[item.Slug] > 1 && (item.Pid != null && item.Pid < headings.Count && headings[item.Id].Slug.StartsWith(item.Slug))) {
        //         item.Slug = $"{item.Slug}-{slugCounts[item.Slug] - 1}";
        //     }   
        // }
        //
        //
        //
        // List<TocNode>? GetNodes(int pid = -1) {
        //     var nodes = headings.Where(a => a.Pid == pid).ToList();
        //     return nodes.Count == 0
        //         ? null
        //         : nodes.Select(a => new TocNode {Text = a.Text, Href = $"#{a.Slug}", Nodes = GetNodes(a.Id)}).ToList();
        // }
        //
        // return GetNodes();
    }

    public static List<TocNode>? ExtractToc(this Post post) {
        if (post.Content == null) return null;
        var doc = Markdig.Markdown.Parse(post.Content);
        return doc.ExtractToc();
    }
}