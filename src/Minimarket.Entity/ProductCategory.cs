using System;
using System.Collections.Generic;

namespace Minimarket.Entity;

public partial class ProductCategory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? RegisterDate { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
