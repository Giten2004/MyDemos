using System;
using System.Linq;
using System.Web.Mvc;
using VM.Mvc.Extensions;
using VM.Models;
using VM.Services;
using Microsoft.Practices.Unity.Utility;
using VM;

namespace VM.Controllers
{
    public class OrderController : VmController
    {
        /// <summary>
        /// 实现了定购业务逻辑的OrderService
        /// </summary>
        /// <remarks>
        /// 1、属性类型是接口IOrderService
        /// 2、属性通过构造器注入（Constructor Injection）的方式进行初始化
        /// </remarks>
        public IOrderService OrderService { get; private set; }
        public OrderController(IOrderService orderService)
        {
            this.OrderService = orderService;
            this.AddDisposableObject(orderService);
        }

        /// <summary>
        /// 显示购物车商品列表
        /// </summary>
        /// <remarks>
        /// 输入参数通过自定义<see cref="ShoppingCartBinder"/>进行绑定
        /// </remarks>     
        public ActionResult ShoppingCart([ModelBinder(typeof(ShoppingCartBinder))]ShoppingCart shoppingCart)
        {
            return View(shoppingCart);
        }


        /// <summary>
        /// 添加新的商品到购物车，并显示当前购物车中商品列表
        /// </summary>
        [HttpPost]
        public ActionResult ShoppingCart(string productId, string productName, decimal price, [ModelBinder(typeof(ShoppingCartBinder))]ShoppingCart shoppingCart)
        {
            shoppingCart.Add(productId, productName, price);
            return View(ShoppingCartBinder.GetShoppingCart());
        }

        /// <summary>
        /// 删除购物车中某个商品
        /// </summary>
        [HttpPost]
        public ActionResult Remove(string productId)
        {
            ShoppingCart shoppingCart = ShoppingCartBinder.GetShoppingCart();
            ShoppingCartItem cartItem = shoppingCart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (null != cartItem)
            {
                shoppingCart.Items.Remove(cartItem);
            }
            return RedirectToAction("ShoppingCart");
        }

        [Authorize]
        [HandleErrorAction("OnCheckOutError")]
        public ActionResult CheckOut([ModelBinder(typeof(ShoppingCartBinder))]ShoppingCart shoppingCart)
        {
            Guard.ArgumentNotNull(shoppingCart, "shoppingCart");
            Order order = new Order
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderTime = DateTime.Now,
                UserName = User.Identity.Name
            };
            foreach (ShoppingCartItem item in shoppingCart.Items)
            {
                order.OrderLines.Add(new OrderLine
                {
                    Order = order,
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }
            this.OrderService.SubmitOrder(order);
            ShoppingCartBinder.Clear();
            return View();
        }

        /// <summary>
        /// 用于处理从Action方法CheckOut抛出的异常
        /// </summary>
        /// <remarks>
        /// 1、当前状态（购物车商品列表）会被保持
        /// 2、错误消息会显示在ValidationSummary中
        /// </remarks>
        public ActionResult OnCheckOutError([ModelBinder(typeof(ShoppingCartBinder))]ShoppingCart shoppingCart)
        {
            return View("ShoppingCart", shoppingCart);
        }
    }
}