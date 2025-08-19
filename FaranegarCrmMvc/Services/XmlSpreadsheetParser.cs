using System.Xml.Linq;

namespace FaranegarCrmMvc.Services;

/// <summary>Parser for Excel 2003 XML (SpreadsheetML) exported by Faranegar.</summary>
public static class XmlSpreadsheetParser
{
    static readonly XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";

    public static List<Dictionary<string, string>> Parse(Stream xmlStream)
    {
        var doc = XDocument.Load(xmlStream);
        var table = doc.Descendants(ss + "Worksheet").First().Descendants(ss + "Table").First();
        var rows = table.Descendants(ss + "Row").ToList();
        if (rows.Count == 0) return new();

        var headers = GetRowValues(rows[0]); // first row = header
        var list = new List<Dictionary<string, string>>();

        for (int i = 1; i < rows.Count; i++)
        {
            var values = GetRowValues(rows[i]);
            var dict = new Dictionary<string, string>();
            for (int c = 0; c < headers.Count; c++)
            {
                var key = headers[c]?.Trim() ?? $"col_{c + 1}";
                var val = c < values.Count ? (values[c] ?? string.Empty) : string.Empty;
                dict[key] = val.Trim();
            }
            if (dict.Values.All(v => string.IsNullOrWhiteSpace(v))) continue;
            list.Add(dict);
        }
        return list;
    }

    private static List<string?> GetRowValues(XElement row)
        => row.Descendants(ss + "Cell").Select(c => c.Descendants(ss + "Data").FirstOrDefault()?.Value).ToList();
}
