using System;
using System.Collections.Generic;

namespace Minimarket.Entity;

public partial class Sale
{
    public int Id { get; set; }

    public string? DocumentNumber { get; set; }

    public string? PaidType { get; set; }

    public decimal? Total { get; set; }

    public DateTime? RegisterDate { get; set; }

    public virtual ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
}
