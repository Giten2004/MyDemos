using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcApp.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<string, int> stock = new Dictionary<string, int>();

        static HomeController()
        {
            stock.Add("001", 20);
            stock.Add("002", 30);
            stock.Add("003", 40);
        }

        public ActionResult Index()
        {
            ShoppingCart cart = new ShoppingCart();
            cart.Add(new ShoppingCartItem
            {
                Id = "001",
                Quantity = 1,
                Name = "商品A"
            });
            cart.Add(new ShoppingCartItem
            {
                Id = "002",
                Quantity = 1,
                Name = "商品B"
            });
            cart.Add(new ShoppingCartItem
            {
                Id = "003",
                Quantity = 1,
                Name = "商品C"
            });
            return View(cart);
        }

        public ActionResult ProcessOrder(ShoppingCart cart)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var cartItem in cart)
            {
                if (!CheckStock(cartItem.Id, cartItem.Quantity))
                {
                    sb.Append(string.Format("{0}: {1};", cartItem.Name, stock[cartItem.Id]));
                }
            }
            if (string.IsNullOrEmpty(sb.ToString()))
            {
                return Content("alert('购物订单成功处理！');", "text/javascript");
            }
            string script = string.Format("alert('库存不足! ({0})');",sb.ToString().TrimEnd(';'));
            return JavaScript(script);
        }

        private bool CheckStock(string id, int quantity)
        {
            return stock[id] >= quantity;
        }
    }
}