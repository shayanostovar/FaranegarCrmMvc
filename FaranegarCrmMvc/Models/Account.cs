using System.ComponentModel.DataAnnotations;

namespace FaranegarCrmMvc.Models;

public class Account
{
    public int Id { get; set; }
    [MaxLength(100)] public string? Code { get; set; }
    [Required, MaxLength(200)] public string Name { get; set; } = default!;
    [MaxLength(100)] public string? Phone { get; set; }
    [MaxLength(200)] public string? Email { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
