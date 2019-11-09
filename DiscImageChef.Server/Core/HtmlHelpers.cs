using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DiscImageChef.Server.Core
{
    public static class HtmlHelpers
    {
        public static string EncodedMultiLineText(this IHtmlHelper<CommonTypes.Metadata.Ata> helper, string text) =>
            string.IsNullOrEmpty(text) ? string.Empty
                : Regex.Replace(helper.Encode(text), "&#xD;&#xA;|&#xA;|&#xD;", "<br/>");
    }
}