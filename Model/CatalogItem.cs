using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using eShop.Catalog.API.Exceptions;

namespace eShop.Catalog.API.Model;

/// <summary>
/// Represents a catalog item.
/// </summary>
public class CatalogItem
{
    /// <summary>
    /// Unique identifier for the catalog item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the catalog item.
    /// </summary>
    [Required]
    [MaxLength(100)]
    [MinLength(3, ErrorMessage = "{0} must be at least 3 characters long.")]
    public string? Name { get; set; }

    /// <summary>
    /// Description of the catalog item.
    /// </summary>
    [Required]
    [StringLength(500, MinimumLength = 10)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, 10000)]
    public decimal Price { get; set; }

    public string? PictureFileName { get; set; }

    public int CatalogTypeId { get; set; }

    [Required]
    public CatalogType? CatalogType { get; set; }

    public int CatalogBrandId { get; set; }

    [Required]
    public CatalogBrand? CatalogBrand { get; set; }

    /// <summary>
    /// Quantity in stock
    /// </summary>
    public int AvailableStock { get; set; }

    // Available stock at which we should reorder
    public int RestockThreshold { get; set; }

    // Maximum number of units that can be in-stock at any time (due to physicial/logistical constraints in warehouses)
    public int MaxStockThreshold { get; set; }

    /// <summary>
    /// True if item is on reorder
    /// </summary>
    public bool OnReorder { get; set; }

    public CatalogItem() { }


    /// <summary>
    /// Decrements the quantity of a particular item in inventory and ensures the restockThreshold hasn't
    /// been breached. If so, a RestockRequest is generated in CheckThreshold.
    ///
    /// If there is sufficient stock of an item, then the integer returned at the end of this call should be the same as quantityDesired.
    /// In the event that there is not sufficient stock available, the method will remove whatever stock is available and return that quantity to the client.
    /// In this case, it is the responsibility of the client to determine if the amount that is returned is the same as quantityDesired.
    /// It is invalid to pass in a negative number.
    /// </summary>
    /// <param name="quantityDesired"></param>
    /// <returns>int: Returns the number actually removed from stock. </returns>
    ///
    public int RemoveStock(int quantityDesired)
    {
        if (AvailableStock == 0)
        {
            throw new CatalogDomainException($"Empty stock, product item {Name} is sold out");
        }

        if (quantityDesired <= 0)
        {
            throw new CatalogDomainException($"Item units desired should be greater than zero");
        }

        int removed = Math.Min(quantityDesired, this.AvailableStock);

        this.AvailableStock -= removed;

        return removed;
    }

    /// <summary>
    /// Increments the quantity of a particular item in inventory.
    /// <param name="quantity"></param>
    /// <returns>int: Returns the quantity that has been added to stock</returns>
    /// </summary>
    public int AddStock(int quantity)
    {
        int original = this.AvailableStock;

        // The quantity that the client is trying to add to stock is greater than what can be physically accommodated in the Warehouse
        if ((this.AvailableStock + quantity) > this.MaxStockThreshold)
        {
            // For now, this method only adds new units up maximum stock threshold. In an expanded version of this application, we
            //could include tracking for the remaining units and store information about overstock elsewhere.
            this.AvailableStock += (this.MaxStockThreshold - this.AvailableStock);
        }
        else
        {
            this.AvailableStock += quantity;
        }

        this.OnReorder = false;

        return this.AvailableStock - original;
    }
}