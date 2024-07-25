using System;
using System.Collections.Generic;

namespace Custom.Database.Data;

public partial class Topping
{
    public int Id { get; set; }

    public int FK_Pizza { get; set; }

    public string? Description { get; set; }

    public virtual Pizza FK_PizzaNavigation { get; set; } = null!;
}
