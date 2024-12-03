using System;
using System.Collections.Generic;

namespace PRN212_Prj.Models;

public partial class Table
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
