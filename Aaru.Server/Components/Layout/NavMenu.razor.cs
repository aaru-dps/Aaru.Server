using Markdig;
using Microsoft.AspNetCore.Components.Routing;

namespace Aaru.Server.New.Components.Layout;

public partial class NavMenu
{
    string? _currentUrl;
    string  _sidebarMarkup = "";

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }

    protected override void OnInitialized()
    {
        _currentUrl                       =  NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        NavigationManager.LocationChanged += OnLocationChanged;

        string? docFolder = Configuration.GetSection("DocumentationFolders").GetValue<string>("Stable");

        if(docFolder is null) return;

        string docPath = Path.Combine(HostEnvironment.ContentRootPath, docFolder);

        if(!Directory.Exists(docPath)) return;

        string sidebarPath = Path.Combine(docPath, "_sidebar.md");

        if(!File.Exists(sidebarPath)) return;

        string sidebarContents = File.ReadAllText(sidebarPath);

        _sidebarMarkup = Markdown.ToHtml(sidebarContents);
    }

    void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        _currentUrl = NavigationManager.ToBaseRelativePath(e.Location);
        StateHasChanged();
    }
}