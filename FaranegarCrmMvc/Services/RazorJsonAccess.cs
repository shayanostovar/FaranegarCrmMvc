using System.Text.Json;
using FaranegarCrmMvc.Models;

namespace FaranegarCrmMvc.Services;

public static class RazorJsonAccess
{
    public static List<string> GetHeadersFromJson(string json)
    {
        var list = new List<string>();
        using var doc = JsonDocument.Parse(json);
        foreach (var p in doc.RootElement.EnumerateObject())
            list.Add(p.Name);
        return list;
    }

    public static string? GetValue(SalesReport r, string header)
    {
        if (string.IsNullOrWhiteSpace(r.AdditionalJson)) return null;
        using var doc = JsonDocument.Parse(r.AdditionalJson);
        if (doc.RootElement.TryGetProperty(header, out var prop))
            return prop.ValueKind == JsonValueKind.String ? prop.GetString() : prop.ToString();

        // نرمال‌سازی ی/ک
        string Norm(string s) => s.Replace("ي", "ی").Replace("ك", "ک").Replace("\u200c", "").Replace("‌", "").Trim();
        var target = Norm(header);
        foreach (var p in doc.RootElement.EnumerateObject())
            if (Norm(p.Name) == target)
                return p.Value.ValueKind == JsonValueKind.String ? p.Value.GetString() : p.Value.ToString();

        return null;
    }
}
