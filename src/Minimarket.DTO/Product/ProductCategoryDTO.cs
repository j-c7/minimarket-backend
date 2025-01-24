using System.ComponentModel.DataAnnotations;

namespace Minimarket.DTO.Product;

public class ProductCategoryDTO
{
    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string? Name { get; set; }
}