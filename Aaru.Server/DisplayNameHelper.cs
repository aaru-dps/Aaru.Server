using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Aaru.Server;

public static class DisplayNameHelper
{
    public static string GetDisplayName(Type type, string propertyName)
    {
        PropertyInfo? prop = type.GetProperty(propertyName);

        if(prop is null) return propertyName;

        DisplayAttribute? displayAttr = prop.GetCustomAttributes(typeof(DisplayAttribute), true)
                                            .OfType<DisplayAttribute>()
                                            .FirstOrDefault();

        return displayAttr?.Name ?? propertyName;
    }
}