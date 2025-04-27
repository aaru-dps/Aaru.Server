using Markdig;
using Markdig.Prism;
using Microsoft.AspNetCore.Components;

namespace Aaru.Server.Components.Pages;

public partial class Documentation
{
    readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions()
                                                                       .UseEmojiAndSmiley()
                                                                       .UseBootstrap()
                                                                       .UsePrism()
                                                                       .Build();
    string? _documentMarkup;
    bool    _notFound = true;
    [CascadingParameter]
    HttpContext HttpContext { get; set; } = default!;

    [Parameter]
    public string? DocumentName { get; set; }
    [Parameter]
    public string? DocumentCategory { get; set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        base.OnInitialized();


        HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        DocumentCategory ??= "";

        if(DocumentName is null) return;

        string sanitizedDocumentCategory = Path.GetFileName(DocumentCategory).ToLowerInvariant();
        string sanitizedDocumentName     = Path.GetFileName(DocumentName).ToLowerInvariant() + ".md";

        string? docFolder = Configuration.GetSection("DocumentationFolders").GetValue<string>("Stable");

        if(docFolder is null) return;

        string docPath = Path.Combine(HostEnvironment.ContentRootPath, docFolder, sanitizedDocumentCategory);

        if(!Directory.Exists(docPath)) return;

        string document = Path.Combine(docPath, sanitizedDocumentName);

        if(!File.Exists(document)) return;

        string documentText = File.ReadAllText(document);

        _documentMarkup = Markdown.ToHtml(documentText, _pipeline);

        _notFound                       = false;
        HttpContext.Response.StatusCode = StatusCodes.Status200OK;
    }
}