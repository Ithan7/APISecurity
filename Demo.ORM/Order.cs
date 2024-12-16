using System;
using System.Collections.Generic;

namespace Demo.ORM;

public partial class Order
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }
}
