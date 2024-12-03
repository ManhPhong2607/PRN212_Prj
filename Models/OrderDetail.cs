using System;
using System.Collections.Generic;

namespace PRN212_Prj.Models;

public partial class OrderDetail
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? MenuId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Price { get; set; }

    public virtual Menu? Menu { get; set; }

    public virtual Order? Order { get; set; }
    public decimal Total => (Quantity ?? 0) * (Price ?? 0);
}
