using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VM.Models
{
public class ShoppingCartBinder: IModelBinder
{
    private const string KeyOfShoppingCart = "VM.ShoppingCart";
    public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    {
        return GetShoppingCart();
    }

    public static ShoppingCart GetShoppingCart()
    {
        if (HttpContext.Current.Session[KeyOfShoppingCart] == null)
        {
            HttpContext.Current.Session[KeyOfShoppingCart] = new ShoppingCart();
        }
        return (ShoppingCart)HttpContext.Current.Session[KeyOfShoppingCart];
    }

    public static void Clear()
    {
        HttpContext.Current.Session[KeyOfShoppingCart] = new ShoppingCart();
    }
}
}