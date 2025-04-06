using System.ComponentModel.DataAnnotations;

namespace eShop.Catalog.API;

public class CatalogOptions
{
    [Required]
    public string? PicBaseUrl { get; set; }
    public bool UseCustomizationData { get; set; }
}