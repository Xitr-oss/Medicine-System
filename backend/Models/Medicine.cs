using System;
using System.Collections.Generic;

namespace backend.Models;

public partial class Medicine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    public decimal Price { get; set; }

    public string? Description { get; set; }

    public int Stock { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();
}
