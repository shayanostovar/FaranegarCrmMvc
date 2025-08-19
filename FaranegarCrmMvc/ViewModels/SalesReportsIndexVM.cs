namespace FaranegarCrmMvc.ViewModels;

using FaranegarCrmMvc.Models;

public class SalesReportsIndexVM
{
    public List<string> Headers { get; set; } = new();
    public List<SalesReport> Data { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}
