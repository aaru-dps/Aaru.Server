// /***************************************************************************
// Aaru Data Preservation Suite
// ----------------------------------------------------------------------------
//
// Filename       : SpectreMarkupConverter.cs
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

using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Aaru.Server.Services;

/// <summary>
///     Converts Spectre Console markup to HTML
/// </summary>
public static partial class SpectreMarkupConverter
{
    // Matches color formats like:
    // "red on blue", "#ff0000 on blue", "rgb(255,0,0) on blue", etc.
    private static readonly Regex _colorMarkupRegex = ColorRegex();

    // Static mapping of Spectre Console color names to hex values
    private static readonly Dictionary<string, string> _spectreColorMap = new(StringComparer.OrdinalIgnoreCase)
    {
        // Standard colors (0-15)
        ["black"]   = "#000000",
        ["maroon"]  = "#800000",
        ["green"]   = "#008000",
        ["olive"]   = "#808000",
        ["navy"]    = "#000080",
        ["purple"]  = "#800080",
        ["teal"]    = "#008080",
        ["silver"]  = "#c0c0c0",
        ["grey"]    = "#808080",
        ["red"]     = "#ff0000",
        ["lime"]    = "#00ff00",
        ["yellow"]  = "#ffff00",
        ["blue"]    = "#0000ff",
        ["fuchsia"] = "#ff00ff",
        ["aqua"]    = "#00ffff",
        ["white"]   = "#ffffff",

        // Extended colors (16-255)
        ["grey0"]             = "#000000",
        ["navyblue"]          = "#00005f",
        ["darkblue"]          = "#000087",
        ["blue3"]             = "#0000af",
        ["blue3_1"]           = "#0000d7",
        ["blue1"]             = "#0000ff",
        ["darkgreen"]         = "#005f00",
        ["deepskyblue4"]      = "#005f5f",
        ["deepskyblue4_1"]    = "#005f87",
        ["deepskyblue4_2"]    = "#005faf",
        ["dodgerblue3"]       = "#005fd7",
        ["dodgerblue2"]       = "#005fff",
        ["green4"]            = "#008700",
        ["springgreen4"]      = "#00875f",
        ["turquoise4"]        = "#008787",
        ["deepskyblue3"]      = "#0087af",
        ["deepskyblue3_1"]    = "#0087d7",
        ["dodgerblue1"]       = "#0087ff",
        ["green3"]            = "#00af00",
        ["springgreen3"]      = "#00af5f",
        ["darkcyan"]          = "#00af87",
        ["lightseagreen"]     = "#00afaf",
        ["deepskyblue2"]      = "#00afd7",
        ["deepskyblue1"]      = "#00afff",
        ["green3_1"]          = "#00d700",
        ["springgreen3_1"]    = "#00d75f",
        ["springgreen2"]      = "#00d787",
        ["cyan3"]             = "#00d7af",
        ["darkturquoise"]     = "#00d7d7",
        ["turquoise2"]        = "#00d7ff",
        ["green1"]            = "#00ff00",
        ["springgreen2_1"]    = "#00ff5f",
        ["springgreen1"]      = "#00ff87",
        ["mediumspringgreen"] = "#00ffaf",
        ["cyan2"]             = "#00ffd7",
        ["cyan1"]             = "#00ffff",
        ["darkred"]           = "#5f0000",
        ["deeppink4"]         = "#5f005f",
        ["purple4"]           = "#5f0087",
        ["purple4_1"]         = "#5f00af",
        ["purple3"]           = "#5f00d7",
        ["blueviolet"]        = "#5f00ff",
        ["orange4"]           = "#5f5f00",
        ["grey37"]            = "#5f5f5f",
        ["mediumpurple4"]     = "#5f5f87",
        ["slateblue3"]        = "#5f5faf",
        ["slateblue3_1"]      = "#5f5fd7",
        ["royalblue1"]        = "#5f5fff",
        ["chartreuse4"]       = "#5f8700",
        ["darkseagreen4"]     = "#5f875f",
        ["paleturquoise4"]    = "#5f8787",
        ["steelblue"]         = "#5f87af",
        ["steelblue3"]        = "#5f87d7",
        ["cornflowerblue"]    = "#5f87ff",
        ["chartreuse3"]       = "#5faf00",
        ["darkseagreen4_1"]   = "#5faf5f",
        ["cadetblue"]         = "#5faf87",
        ["cadetblue_1"]       = "#5fafaf",
        ["skyblue3"]          = "#5fafd7",
        ["steelblue1"]        = "#5fafff",
        ["chartreuse3_1"]     = "#5fd700",
        ["palegreen3"]        = "#5fd75f",
        ["seagreen3"]         = "#5fd787",
        ["aquamarine3"]       = "#5fd7af",
        ["mediumturquoise"]   = "#5fd7d7",
        ["steelblue1_1"]      = "#5fd7ff",
        ["chartreuse2"]       = "#5fff00",
        ["seagreen2"]         = "#5fff5f",
        ["seagreen1"]         = "#5fff87",
        ["seagreen1_1"]       = "#5fffaf",
        ["aquamarine1"]       = "#5fffd7",
        ["darkslategray2"]    = "#5fffff",
        ["darkred_1"]         = "#870000",
        ["deeppink4_1"]       = "#87005f",
        ["darkmagenta"]       = "#870087",
        ["darkmagenta_1"]     = "#8700af",
        ["darkviolet"]        = "#8700d7",
        ["purple_1"]          = "#8700ff",
        ["orange4_1"]         = "#875f00",
        ["lightpink4"]        = "#875f5f",
        ["plum4"]             = "#875f87",
        ["mediumpurple3"]     = "#875faf",
        ["mediumpurple3_1"]   = "#875fd7",
        ["slateblue1"]        = "#875fff",
        ["yellow4"]           = "#878700",
        ["wheat4"]            = "#87875f",
        ["grey53"]            = "#878787",
        ["lightslategrey"]    = "#8787af",
        ["mediumpurple"]      = "#8787d7",
        ["lightslateblue"]    = "#8787ff",
        ["yellow4_1"]         = "#87af00",
        ["darkolivegreen3"]   = "#87af5f",
        ["darkseagreen"]      = "#87af87",
        ["lightskyblue3"]     = "#87afaf",
        ["lightskyblue3_1"]   = "#87afd7",
        ["skyblue2"]          = "#87afff",
        ["chartreuse2_1"]     = "#87d700",
        ["darkolivegreen3_1"] = "#87d75f",
        ["palegreen3_1"]      = "#87d787",
        ["darkseagreen3"]     = "#87d7af",
        ["darkslategray3"]    = "#87d7d7",
        ["skyblue1"]          = "#87d7ff",
        ["chartreuse1"]       = "#87ff00",
        ["lightgreen"]        = "#87ff5f",
        ["lightgreen_1"]      = "#87ff87",
        ["palegreen1"]        = "#87ffaf",
        ["aquamarine1_1"]     = "#87ffd7",
        ["darkslategray1"]    = "#87ffff",
        ["red3"]              = "#af0000",
        ["deeppink4_2"]       = "#af005f",
        ["mediumvioletred"]   = "#af0087",
        ["magenta3"]          = "#af00af",
        ["darkviolet_1"]      = "#af00d7",
        ["purple_2"]          = "#af00ff",
        ["darkorange3"]       = "#af5f00",
        ["indianred"]         = "#af5f5f",
        ["hotpink3"]          = "#af5f87",
        ["mediumorchid3"]     = "#af5faf",
        ["mediumorchid"]      = "#af5fd7",
        ["mediumpurple2"]     = "#af5fff",
        ["darkgoldenrod"]     = "#af8700",
        ["lightsalmon3"]      = "#af875f",
        ["rosybrown"]         = "#af8787",
        ["grey63"]            = "#af87af",
        ["mediumpurple2_1"]   = "#af87d7",
        ["mediumpurple1"]     = "#af87ff",
        ["gold3"]             = "#afaf00",
        ["darkkhaki"]         = "#afaf5f",
        ["navajowhite3"]      = "#afaf87",
        ["grey69"]            = "#afafaf",
        ["lightsteelblue3"]   = "#afafd7",
        ["lightsteelblue"]    = "#afafff",
        ["yellow3"]           = "#afd700",
        ["darkolivegreen3_2"] = "#afd75f",
        ["darkseagreen3_1"]   = "#afd787",
        ["darkseagreen2"]     = "#afd7af",
        ["lightcyan3"]        = "#afd7d7",
        ["lightskyblue1"]     = "#afd7ff",
        ["greenyellow"]       = "#afff00",
        ["darkolivegreen2"]   = "#afff5f",
        ["palegreen1_1"]      = "#afff87",
        ["darkseagreen2_1"]   = "#afffaf",
        ["darkseagreen1"]     = "#afffd7",
        ["paleturquoise1"]    = "#afffff",
        ["red3_1"]            = "#d70000",
        ["deeppink3"]         = "#d7005f",
        ["deeppink3_1"]       = "#d70087",
        ["magenta3_1"]        = "#d700af",
        ["magenta3_2"]        = "#d700d7",
        ["magenta2"]          = "#d700ff",
        ["darkorange3_1"]     = "#d75f00",
        ["indianred_1"]       = "#d75f5f",
        ["hotpink3_1"]        = "#d75f87",
        ["hotpink2"]          = "#d75faf",
        ["orchid"]            = "#d75fd7",
        ["mediumorchid1"]     = "#d75fff",
        ["orange3"]           = "#d78700",
        ["lightsalmon3_1"]    = "#d7875f",
        ["lightpink3"]        = "#d78787",
        ["pink3"]             = "#d787af",
        ["plum3"]             = "#d787d7",
        ["violet"]            = "#d787ff",
        ["gold3_1"]           = "#d7af00",
        ["lightgoldenrod3"]   = "#d7af5f",
        ["tan"]               = "#d7af87",
        ["mistyrose3"]        = "#d7afaf",
        ["thistle3"]          = "#d7afd7",
        ["plum2"]             = "#d7afff",
        ["yellow3_1"]         = "#d7d700",
        ["khaki3"]            = "#d7d75f",
        ["lightgoldenrod2"]   = "#d7d787",
        ["lightyellow3"]      = "#d7d7af",
        ["grey84"]            = "#d7d7d7",
        ["lightsteelblue1"]   = "#d7d7ff",
        ["yellow2"]           = "#d7ff00",
        ["darkolivegreen1"]   = "#d7ff5f",
        ["darkolivegreen1_1"] = "#d7ff87",
        ["darkseagreen1_1"]   = "#d7ffaf",
        ["honeydew2"]         = "#d7ffd7",
        ["lightcyan1"]        = "#d7ffff",
        ["red1"]              = "#ff0000",
        ["deeppink2"]         = "#ff005f",
        ["deeppink1"]         = "#ff0087",
        ["deeppink1_1"]       = "#ff00af",
        ["magenta2_1"]        = "#ff00d7",
        ["magenta1"]          = "#ff00ff",
        ["orangered1"]        = "#ff5f00",
        ["indianred1"]        = "#ff5f5f",
        ["indianred1_1"]      = "#ff5f87",
        ["hotpink"]           = "#ff5faf",
        ["hotpink_1"]         = "#ff5fd7",
        ["mediumorchid1_1"]   = "#ff5fff",
        ["darkorange"]        = "#ff8700",
        ["salmon1"]           = "#ff875f",
        ["lightcoral"]        = "#ff8787",
        ["palevioletred1"]    = "#ff87af",
        ["orchid2"]           = "#ff87d7",
        ["orchid1"]           = "#ff87ff",
        ["orange1"]           = "#ffaf00",
        ["sandybrown"]        = "#ffaf5f",
        ["lightsalmon1"]      = "#ffaf87",
        ["lightpink1"]        = "#ffafaf",
        ["pink1"]             = "#ffafd7",
        ["plum1"]             = "#ffafff",
        ["gold1"]             = "#ffd700",
        ["lightgoldenrod2_1"] = "#ffd75f",
        ["lightgoldenrod2_2"] = "#ffd787",
        ["navajowhite1"]      = "#ffd7af",
        ["mistyrose1"]        = "#ffd7d7",
        ["thistle1"]          = "#ffd7ff",
        ["yellow1"]           = "#ffff00",
        ["lightgoldenrod1"]   = "#ffff5f",
        ["khaki1"]            = "#ffff87",
        ["wheat1"]            = "#ffffaf",
        ["cornsilk1"]         = "#ffffd7",
        ["grey100"]           = "#ffffff",
        ["grey3"]             = "#080808",
        ["grey7"]             = "#121212",
        ["grey11"]            = "#1c1c1c",
        ["grey15"]            = "#262626",
        ["grey19"]            = "#303030",
        ["grey23"]            = "#3a3a3a",
        ["grey27"]            = "#444444",
        ["grey30"]            = "#4e4e4e",
        ["grey35"]            = "#585858",
        ["grey39"]            = "#626262",
        ["grey42"]            = "#6c6c6c",
        ["grey46"]            = "#767676",
        ["grey50"]            = "#808080",
        ["grey54"]            = "#8a8a8a",
        ["grey58"]            = "#949494",
        ["grey62"]            = "#9e9e9e",
        ["grey66"]            = "#a8a8a8",
        ["grey70"]            = "#b2b2b2",
        ["grey74"]            = "#bcbcbc",
        ["grey78"]            = "#c6c6c6",
        ["grey82"]            = "#d0d0d0",
        ["grey85"]            = "#dadada",
        ["grey89"]            = "#e4e4e4",
        ["grey93"]            = "#eeeeee"
    };

    /// <summary>
    ///     Converts Spectre Console markup to HTML
    /// </summary>
    /// <param name="spectreMarkup">Text with Spectre Console markup tags</param>
    /// <returns>HTML string with equivalent styling</returns>
    public static string ToHtml(string spectreMarkup)
    {
        if(string.IsNullOrEmpty(spectreMarkup)) return string.Empty;

        // Normalize line endings to \n
        spectreMarkup = spectreMarkup.Replace("\r\n", "\n").Replace("\r", "\n");

        List<MarkupTag> markups = ParseMarkups(spectreMarkup);

        if(markups.Count == 0)
        {
            string encoded = WebUtility.HtmlEncode(spectreMarkup);

            return encoded.Replace("\n", "<br />");
        }

        var html = new StringBuilder();

        // Create a list of tag ranges to exclude from the text
        var tagRanges = new List<(int start, int end)>();

        foreach(MarkupTag markup in markups)
        {
            // Add opening tag range
            tagRanges.Add((markup.Start, markup.OpenTagEnd));

            // Add closing tag range
            tagRanges.Add((markup.CloseTagStart, markup.End));
        }

        // Create breakpoints at all positions
        var breakpoints = new SortedSet<int>
        {
            0,
            spectreMarkup.Length
        };

        // Add all tag boundaries as breakpoints
        foreach((int start, int end) range in tagRanges)
        {
            breakpoints.Add(range.start);
            breakpoints.Add(range.end);
        }

        var breakpointList = breakpoints.ToList();

        for(var i = 0; i < breakpointList.Count - 1; i++)
        {
            int start = breakpointList[i];
            int end   = breakpointList[i + 1];

            // Skip empty segments
            if(start == end) continue;

            // Skip this segment if it overlaps with any tag range
            bool isInsideTag = tagRanges.Any(range => start >= range.start && start < range.end  ||
                                                      end   > range.start  && end   <= range.end ||
                                                      start <= range.start && end   >= range.end);

            if(isInsideTag) continue;

            // Find which markup tags apply to this content segment
            var applicableMarkups = markups.Where(m => m.OpenTagEnd <= start && m.CloseTagStart >= end)
                                           .OrderBy(static m => m.Start)
                                           .ThenByDescending(static m => m.End)
                                           .ToList();

            string content     = spectreMarkup.Substring(start, end - start);
            string encodedText = WebUtility.HtmlEncode(content);

            // Convert newlines to <br /> tags
            encodedText = encodedText.Replace("\n", "<br />");

            var styles      = new List<string>();
            var htmlTags    = new List<string>();
            var closingTags = new List<string>();

            foreach(MarkupTag markup in applicableMarkups)
            {
                if(markup.Tag.Contains("bold", StringComparison.OrdinalIgnoreCase))
                {
                    htmlTags.Add("<strong>");
                    closingTags.Insert(0, "</strong>");
                }

                if(markup.Tag.Contains("italic", StringComparison.OrdinalIgnoreCase))
                {
                    htmlTags.Add("<em>");
                    closingTags.Insert(0, "</em>");
                }

                if(markup.Tag.Contains("underline", StringComparison.OrdinalIgnoreCase))
                {
                    htmlTags.Add("<u>");
                    closingTags.Insert(0, "</u>");
                }

                if(!_colorMarkupRegex.IsMatch(markup.Tag)) continue;

                Match   match      = _colorMarkupRegex.Match(markup.Tag);
                string  foreground = match.Groups["fg"].Value;
                string? background = match.Groups["bg"].Success ? match.Groups["bg"].Value : null;

                // Apply foreground color
                if(!string.IsNullOrEmpty(foreground))
                {
                    string? fgColor = ParseColor(foreground);

                    if(fgColor != null) styles.Add($"color: {fgColor}");
                }

                // Apply background color
                if(string.IsNullOrEmpty(background)) continue;

                string? bgColor = ParseColor(background);

                if(bgColor != null) styles.Add($"background-color: {bgColor}");
            }

            // Build the HTML segment
            foreach(string tag in htmlTags) html.Append(tag);

            if(styles.Count > 0)
            {
                html.Append("<span style=\"");
                html.Append(string.Join("; ", styles));
                html.Append("\">");
                html.Append(encodedText);
                html.Append("</span>");
            }
            else
                html.Append(encodedText);

            foreach(string tag in closingTags) html.Append(tag);
        }

        return html.ToString();
    }

    static string? ParseColor(string color)
    {
        // Handle hex colors like #ff0000
        if(color.StartsWith('#')) return color;

        // Handle rgb(r,g,b) format
        if(color.StartsWith("rgb(", StringComparison.Ordinal) && color.EndsWith(')'))
            return color; // RGB format is valid in CSS

        // Handle Spectre Console named colors using the mapping
        if(_spectreColorMap.TryGetValue(color, out string? hexValue)) return hexValue;

        // Try common HTML/CSS color names
        return color.ToLowerInvariant() switch
               {
                   "aliceblue"            => "#f0f8ff",
                   "antiquewhite"         => "#faebd7",
                   "aquamarine"           => "#7fffd4",
                   "azure"                => "#f0ffff",
                   "beige"                => "#f5f5dc",
                   "bisque"               => "#ffe4c4",
                   "blanchedalmond"       => "#ffebcd",
                   "blueviolet"           => "#8a2be2",
                   "brown"                => "#a52a2a",
                   "burlywood"            => "#deb887",
                   "cadetblue"            => "#5f9ea0",
                   "chartreuse"           => "#7fff00",
                   "chocolate"            => "#d2691e",
                   "coral"                => "#ff7f50",
                   "cornflowerblue"       => "#6495ed",
                   "cornsilk"             => "#fff8dc",
                   "crimson"              => "#dc143c",
                   "cyan"                 => "#00ffff",
                   "darkblue"             => "#00008b",
                   "darkcyan"             => "#008b8b",
                   "darkgoldenrod"        => "#b8860b",
                   "darkgray"             => "#a9a9a9",
                   "darkgrey"             => "#a9a9a9",
                   "darkgreen"            => "#006400",
                   "darkkhaki"            => "#bdb76b",
                   "darkmagenta"          => "#8b008b",
                   "darkolivegreen"       => "#556b2f",
                   "darkorange"           => "#ff8c00",
                   "darkorchid"           => "#9932cc",
                   "darkred"              => "#8b0000",
                   "darksalmon"           => "#e9967a",
                   "darkseagreen"         => "#8fbc8f",
                   "darkslateblue"        => "#483d8b",
                   "darkslategray"        => "#2f4f4f",
                   "darkslategrey"        => "#2f4f4f",
                   "darkturquoise"        => "#00ced1",
                   "darkviolet"           => "#9400d3",
                   "deeppink"             => "#ff1493",
                   "deepskyblue"          => "#00bfff",
                   "dimgray"              => "#696969",
                   "dimgrey"              => "#696969",
                   "dodgerblue"           => "#1e90ff",
                   "firebrick"            => "#b22222",
                   "floralwhite"          => "#fffaf0",
                   "forestgreen"          => "#228b22",
                   "gainsboro"            => "#dcdcdc",
                   "ghostwhite"           => "#f8f8ff",
                   "gold"                 => "#ffd700",
                   "goldenrod"            => "#daa520",
                   "gray"                 => "#808080",
                   "greenyellow"          => "#adff2f",
                   "honeydew"             => "#f0fff0",
                   "hotpink"              => "#ff69b4",
                   "indianred"            => "#cd5c5c",
                   "indigo"               => "#4b0082",
                   "ivory"                => "#fffff0",
                   "khaki"                => "#f0e68c",
                   "lavender"             => "#e6e6fa",
                   "lavenderblush"        => "#fff0f5",
                   "lawngreen"            => "#7cfc00",
                   "lemonchiffon"         => "#fffacd",
                   "lightblue"            => "#add8e6",
                   "lightcoral"           => "#f08080",
                   "lightcyan"            => "#e0ffff",
                   "lightgoldenrodyellow" => "#fafad2",
                   "lightgray"            => "#d3d3d3",
                   "lightgrey"            => "#d3d3d3",
                   "lightgreen"           => "#90ee90",
                   "lightpink"            => "#ffb6c1",
                   "lightsalmon"          => "#ffa07a",
                   "lightseagreen"        => "#20b2aa",
                   "lightskyblue"         => "#87cefa",
                   "lightslategray"       => "#778899",
                   "lightslategrey"       => "#778899",
                   "lightsteelblue"       => "#b0c4de",
                   "lightyellow"          => "#ffffe0",
                   "limegreen"            => "#32cd32",
                   "linen"                => "#faf0e6",
                   "magenta"              => "#ff00ff",
                   "mediumaquamarine"     => "#66cdaa",
                   "mediumblue"           => "#0000cd",
                   "mediumorchid"         => "#ba55d3",
                   "mediumpurple"         => "#9370db",
                   "mediumseagreen"       => "#3cb371",
                   "mediumslateblue"      => "#7b68ee",
                   "mediumspringgreen"    => "#00fa9a",
                   "mediumturquoise"      => "#48d1cc",
                   "mediumvioletred"      => "#c71585",
                   "midnightblue"         => "#191970",
                   "mintcream"            => "#f5fffa",
                   "mistyrose"            => "#ffe4e1",
                   "moccasin"             => "#ffe4b5",
                   "navajowhite"          => "#ffdead",
                   "oldlace"              => "#fdf5e6",
                   "olivedrab"            => "#6b8e23",
                   "orange"               => "#ffa500",
                   "orangered"            => "#ff4500",
                   "orchid"               => "#da70d6",
                   "palegoldenrod"        => "#eee8aa",
                   "palegreen"            => "#98fb98",
                   "paleturquoise"        => "#afeeee",
                   "palevioletred"        => "#db7093",
                   "papayawhip"           => "#ffefd5",
                   "peachpuff"            => "#ffdab9",
                   "peru"                 => "#cd853f",
                   "pink"                 => "#ffc0cb",
                   "plum"                 => "#dda0dd",
                   "powderblue"           => "#b0e0e6",
                   "rebeccapurple"        => "#663399",
                   "rosybrown"            => "#bc8f8f",
                   "royalblue"            => "#4169e1",
                   "saddlebrown"          => "#8b4513",
                   "salmon"               => "#fa8072",
                   "sandybrown"           => "#f4a460",
                   "seagreen"             => "#2e8b57",
                   "seashell"             => "#fff5ee",
                   "sienna"               => "#a0522d",
                   "skyblue"              => "#87ceeb",
                   "slateblue"            => "#6a5acd",
                   "slategray"            => "#708090",
                   "slategrey"            => "#708090",
                   "snow"                 => "#fffafa",
                   "springgreen"          => "#00ff7f",
                   "steelblue"            => "#4682b4",
                   "tan"                  => "#d2b48c",
                   "thistle"              => "#d8bfd8",
                   "tomato"               => "#ff6347",
                   "turquoise"            => "#40e0d0",
                   "violet"               => "#ee82ee",
                   "wheat"                => "#f5deb3",
                   "whitesmoke"           => "#f5f5f5",
                   "yellowgreen"          => "#9acd32",
                   _                      => null
               };
    }

    static List<MarkupTag> ParseMarkups(string text)
    {
        var result   = new List<MarkupTag>();
        var tagStack = new Stack<(int start, int openTagEnd, string tag)>();
        var i        = 0;

        while(i < text.Length)
        {
            if(text[i] == '[')
            {
                int tagStart = i;
                i++;

                // Check if it's a closing tag [/]
                if(i < text.Length && text[i] == '/')
                {
                    i++;

                    if(i >= text.Length || text[i] != ']') continue;

                    // Found [/], close the most recent tag
                    if(tagStack.Count > 0)
                    {
                        (int openStart, int openTagEnd, string tag) = tagStack.Pop();
                        int closeTagEnd = i + 1; // After the ']' of [/]
                        result.Add(new MarkupTag(openStart, closeTagEnd, tag, openTagEnd, tagStart));
                    }
                }
                else
                {
                    // Parse opening tag like [red], [bold], etc.
                    int tagNameStart = i;

                    while(i < text.Length && text[i] != ']' && text[i] != '[') i++;

                    if(i >= text.Length || text[i] != ']') continue;

                    string tagName = text.Substring(tagNameStart, i - tagNameStart);

                    if(!string.IsNullOrWhiteSpace(tagName))
                    {
                        int openTagEnd = i + 1; // After the ']'
                        tagStack.Push((tagStart, openTagEnd, tagName));
                    }
                }
            }

            i++;
        }

        return result;
    }

    [GeneratedRegex(@"^(?<fg>#[0-9a-fA-F]{6}|rgb\(\d{1,3},\d{1,3},\d{1,3}\)|[a-zA-Z0-9_]+)(\s+on\s+(?<bg>#[0-9a-fA-F]{6}|rgb\(\d{1,3},\d{1,3},\d{1,3}\)|[a-zA-Z0-9_]+))?$",
                    RegexOptions.Compiled)]
    private static partial Regex ColorRegex();

    sealed record MarkupTag(int Start, int End, string Tag, int OpenTagEnd, int CloseTagStart);
}