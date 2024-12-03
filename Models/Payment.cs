using System;
using System.Collections.Generic;

namespace PRN212_Prj.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string? PaymentMethod { get; set; }

    public DateTime? Date { get; set; }

    public int? OrderId { get; set; }

    public decimal? Total { get; set; }

    public virtual Order? Order { get; set; }
}
