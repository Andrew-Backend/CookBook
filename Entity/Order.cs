using System;
using System.Collections.Generic;

namespace Entity;

public class Order
{
    public int Id { get; set; }
    public DateTime DateOrders { get; set; }
    public string Result { get; set; }

    public List<Ordered> Items { get; set; } = new List<Ordered>();
}