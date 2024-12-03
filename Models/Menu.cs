using System;
using System.Collections.Generic;

namespace PRN212_Prj.Models;

public partial class Menu
{
    public int Id { get; set; }

    public int? CateId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public virtual Category? Cate { get; set; }

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
