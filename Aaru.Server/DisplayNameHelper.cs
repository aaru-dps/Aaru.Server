using System.ComponentModel;
using System.Reflection;

namespace Aaru.Server;

public static class DisplayNameHelper
{
    public static string GetDisplayName(Type type, string propertyName)
    {
        PropertyInfo? prop = type.GetProperty(propertyName);

        if(prop is null) return propertyName;

        DisplayNameAttribute? displayAttr = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true)
                                                .OfType<DisplayNameAttribute>()
                                                .FirstOrDefault();

        return displayAttr?.DisplayName ?? propertyName;
    }
}