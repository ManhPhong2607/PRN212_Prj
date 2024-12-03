using System;
using System.Collections.Generic;

namespace PRN212_Prj.Models;

public partial class CustomerOrder
{
    public int Id { get; set; }

    public int? MenuId { get; set; }

    public int? TableId { get; set; }

    public int? Quantity { get; set; }

    public virtual Menu? Menu { get; set; }

    public virtual Table? Table { get; set; }

    public decimal Total => (Quantity ?? 0) * (Menu?.Price ?? 0);
}
