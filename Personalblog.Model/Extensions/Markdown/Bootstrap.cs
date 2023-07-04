using Markdig;
using Markdig.Extensions.Figures;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Personalblog.Model.Extensions.Markdown;

public class Bootstrap5Extension : IMarkdownExtension {
    public void Setup(MarkdownPipelineBuilder pipeline) {
        // Make sure we don't have a delegate twice
        pipeline.DocumentProcessed -= PipelineOnDocumentProcessed;
        pipeline.DocumentProcessed += PipelineOnDocumentProcessed;
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer) {
    }

    private static void PipelineOnDocumentProcessed(MarkdownDocument document) {
        foreach (var node in document.Descendants()) {
            if (node is Inline) {
                if (node is ContainerInline && node is LinkInline link && link.IsImage) {
                    link.GetAttributes().AddClass("img-fluid");
                }
            }
            else if (node is ContainerBlock) {
                switch (node) {
                    case Table:
                        node.GetAttributes().AddClass("table");
                        break;
                    case QuoteBlock:
                        node.GetAttributes().AddClass("blockquote");
                        break;
                    case Figure:
                        node.GetAttributes().AddClass("figure");
                        break;
                    case ListItemBlock:
                        node.GetAttributes().AddClass("my-1");
                        break;
                }
            }
            else {
                if (node is FigureCaption) {
                    node.GetAttributes().AddClass("figure-caption");
                }
            }
        }
    }
}

public static class MarkdownPipelineExt {
    public static MarkdownPipelineBuilder UseBootstrap5(this MarkdownPipelineBuilder pipeline) {
        pipeline.Extensions.AddIfNotAlready<Bootstrap5Extension>();
        return pipeline;
    }
}