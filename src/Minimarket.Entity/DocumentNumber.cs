using System;
using System.Collections.Generic;

namespace Minimarket.Entity;

public partial class DocumentNumber
{
    public int Id { get; set; }

    public int? LastNumber { get; set; }

    public int? Digits { get; set; }

    public DateTime? RegisterDate { get; set; }
}
