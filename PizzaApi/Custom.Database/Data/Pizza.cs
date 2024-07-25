using System;
using System.Collections.Generic;

namespace Custom.Database.Data;

public partial class Pizza
{
    public int Id { get; set; }

    public string Description { get; set; } = null!;

    public int Diameter { get; set; }

    public double? BakingTime { get; set; }

    public virtual ICollection<Topping> Topping { get; set; } = new List<Topping>();
}
