using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcApp.Models
{
    public class ShoppingCart : List<ShoppingCartItem>
    { }

    public class ShoppingCartItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
