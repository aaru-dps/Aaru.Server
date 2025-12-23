// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : NavMenu.razor.cs
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
using Microsoft.AspNetCore.Components.Routing;

namespace Aaru.Server.Components.Layout;

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