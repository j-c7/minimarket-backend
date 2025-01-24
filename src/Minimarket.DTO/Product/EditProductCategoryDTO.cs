using System.ComponentModel.DataAnnotations;

namespace Minimarket.DTO.Product;

public class EditProductCategoryDTO
{
    [Required(ErrorMessage = "El Id es obligatorio")]
    public int? Id { get; set; }

    [Required(ErrorMessage = "El campo Nombre es obligatorio")]
    public string? Name { get; set; }
}