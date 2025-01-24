using System.ComponentModel.DataAnnotations;

namespace Minimarket.DTO.Product;

public class EditProductDTO
{
    [Required(ErrorMessage = "El Id es obligatorio")]
    public int? Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public int? Stock { get; set; }

    public bool? IsActive { get; set; }
}