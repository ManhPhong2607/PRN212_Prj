using System;
using System.Collections.Generic;

namespace PRN212_Prj.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();
}
