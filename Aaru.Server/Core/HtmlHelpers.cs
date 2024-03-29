using System.Text.RegularExpressions;
using Aaru.CommonTypes.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aaru.Server.Core;

public static class HtmlHelpers
{
    public static string EncodedMultiLineText(this IHtmlHelper<CommonTypes.Metadata.Ata> helper, string text) =>
        string.IsNullOrEmpty(text)
            ? string.Empty
            : Regex.Replace(helper.Encode(text), "&#xD;&#xA;|&#xA;|&#xD;", "<br/>");

    public static string EncodedMultiLineText(this IHtmlHelper<Mmc> helper, string text) =>
        string.IsNullOrEmpty(text)
            ? string.Empty
            : Regex.Replace(helper.Encode(text), "&#xD;&#xA;|&#xA;|&#xD;", "<br/>");

    public static string EncodedMultiLineText(this IHtmlHelper<MmcSd> helper, string text) =>
        string.IsNullOrEmpty(text)
            ? string.Empty
            : Regex.Replace(helper.Encode(text), "&#xD;&#xA;|&#xA;|&#xD;", "<br/>");

    public static string EncodedMultiLineText(this IHtmlHelper<Scsi> helper, string text) =>
        string.IsNullOrEmpty(text)
            ? string.Empty
            : Regex.Replace(helper.Encode(text), "&#xD;&#xA;|&#xA;|&#xD;", "<br/>");
}