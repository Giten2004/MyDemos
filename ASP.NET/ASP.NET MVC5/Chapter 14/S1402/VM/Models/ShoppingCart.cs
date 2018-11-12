using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Unity.Utility;

namespace VM.Models
{
[Serializable]
public class ShoppingCart
{
    public IList<ShoppingCartItem> Items { get; private set; }
    public decimal TotalQuantity
    {
        get { return this.Items.Sum(item => item.Quantity); }
    }
    public decimal TotalPrice
    {
        get { return this.Items.Sum(item => item.Quantity * item.Price); }
    }

    public ShoppingCart()
    {
        Items = new List<ShoppingCartItem>();
    }

    public void Add(string productId, string productName, decimal price)
    {
        Guard.ArgumentNotNullOrEmpty(productId, "productId");
        Guard.ArgumentNotNullOrEmpty(productName, "productName");
        ShoppingCartItem shoppingCartItem = this.Items.FirstOrDefault(item => item.ProductId == productId);
        if (null != shoppingCartItem)
        {
            shoppingCartItem.Quantity++;
        }
        else
        {
            this.Items.Add(new ShoppingCartItem
            {
                ProductId = productId,
                ProductName = productName,
                Quantity = 1,
                Price = price
            });
        }
    }
}

[Serializable]
public class ShoppingCartItem
{
    public string ProductId { get; set; }
    [Display(Name = "片名")]
    public string ProductName { get;  set; }
    [Display(Name = "单价")]
    public decimal Price { get; set; }
    [Display(Name = "数量")]
    public int Quantity { get; set; }
    [Display(Name = "金额")]
    public decimal SubTotalPrice
    {
        get { return this.Quantity * this.Price; }
    }
}
}