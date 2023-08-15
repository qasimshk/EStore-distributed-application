namespace estore.api.Extensions;

using System.Globalization;

public static class GuidExtension
{
    public static int GuidToInteger(this Guid value) =>
        Math.Abs(new Guid(value.ToString()).GetHashCode());

    public static string GuidToString(this Guid value) =>
        value.ToString("N")[..5].ToUpper(CultureInfo.CurrentCulture);
}
