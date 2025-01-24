namespace Minimarket.DTO.Product;

public class ResProductDTO
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CategoryId { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public bool? IsActive { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? RegisterDate { get; set; }

    // ¿Aquí podemos devolver un DTO?
    // public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}