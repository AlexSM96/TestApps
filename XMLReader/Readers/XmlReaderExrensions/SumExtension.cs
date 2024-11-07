using System.Xml.Linq;

namespace XMLReader.Readers.XmlReaderExrensions;

internal static class SumExtension
{
    public static int GetIntSum(this IEnumerable<XElement> elements, string attribute)
    {
        if(string.IsNullOrWhiteSpace(attribute)) return default;
        return elements.Where(x => x.HasAttributes)
               .Sum(x => int.TryParse(x.Attribute(attribute)?.Value, out var result) ? result : 0);
    }

    public static double GetDoubleSum(this IEnumerable<XElement> elements, string attribute)
    {
        if (string.IsNullOrWhiteSpace(attribute)) return default;
        return elements.Where(x => x.HasAttributes)
               .Sum(x => double.TryParse(x.Attribute(attribute)?.Value, out var result) ? result : 0);
    }

    public static long GetLongSum(this IEnumerable<XElement> elements, string attribute)
    {
        if (string.IsNullOrWhiteSpace(attribute)) return default;
        return elements.Where(x => x.HasAttributes)
               .Sum(x => long.TryParse(x.Attribute(attribute)?.Value, out var result) ? result : 0);
    }
}
