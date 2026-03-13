using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Orderitem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int MedicineId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
