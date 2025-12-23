// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : Documentation.razor.cs
// Author(s)      : Natalia Portillo <claunia@claunia.com>
//
// Component      : Aaru Server.
//
// --[ License ] --------------------------------------------------------------
//
//     This library is free software; you can redistribute it and/or modify
//     it under the terms of the GNU Lesser General Public License as
//     published by the Free Software Foundation; either version 2.1 of the
//     License, or (at your option) any later version.
//
//     This library is distributed in the hope that it will be useful, but
//     WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//     Lesser General Public License for more details.
//
//     You should have received a copy of the GNU Lesser General Public
//     License along with this library; if not, see <http://www.gnu.org/licenses/>.
//
// ----------------------------------------------------------------------------
// Copyright © 2011-2026 Natalia Portillo
// ****************************************************************************/

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