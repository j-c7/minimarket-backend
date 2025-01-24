using System.ComponentModel.DataAnnotations;

namespace Minimarket.DTO.Product;

public class ProductDTO
{
    [Required(ErrorMessage = "El Id de la categoria es obligatorio")]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    public decimal? Price { get; set; }

    [Required(ErrorMessage = "El stock es obligatorio")]
    public int? Stock { get; set; }

    [Required(ErrorMessage = "El campo es obligatorio")]
    public bool? IsActive { get; set; }
}