using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace VM
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //主页[影片列表（第1页）]：/
            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new
                {
                    controller = "Product",
                    action = "Index",
                    pageIndex = 1
                });

            //主页[影片列表（第N页）]：/Page1、/Page2、...
            routes.MapRoute(
                name: "Page",
                url: "Page{pageIndex}",
                defaults: new
                {
                    controller = "Product",
                    action = "Index",
                    pageIndex = 1
                },
                constraints: new { pageIndex = @"\d+" });

            //基于指定类型的影片列表（第1页）：/Genre/剧情、/Genre/喜剧、...
            routes.MapRoute(
                name: "GenreHome",
                url: "Genre/{genre}",
                defaults: new
                {
                    controller = "Product",
                    action = "Genre",
                    pageIndex = 1
                });

            //基于指定类型的影片列表（第N页）：/Genre/剧情/Page1、/Genre/剧情/Page2、...
            routes.MapRoute(
                name: "GenrePage",
                url: "Genre/{genre}/Page{pageIndex}",
                defaults: new
                {
                    controller = "Product",
                    action = "Genre",
                    pageIndex = 1
                }
            );

            //由指定演员参演的影片列表（第1页）：/Actor/阿尔•帕西诺
            routes.MapRoute(
                name: "ActorHome",
                url: "Actor/{actor}",
                defaults: new
                {
                    controller = "Product",
                    action = "Actor",
                    pageIndex = 1
                });

            //由指定演员参演的影片列表（第N页）：/Actor/阿尔•帕西诺/Page1、
            //                              /Actor/阿尔•帕西诺/Page2
            routes.MapRoute(
                name: "ActorPage",
                url: "Actor/{actor}/Page{pageIndex}",
                defaults: new
                {
                    controller = "Product",
                    action = "Actor",
                    pageIndex = 1
                });

            //影片详细信息：/魔鬼代言人/006
            routes.MapRoute(
                name: "ProductDetail",
                url: "{productName}/{productId}",
                defaults: new { controller = "Product", action = "Detail" },
                constraints: new { httpMethod = new HttpMethodConstraint("GET") });


            //购物车：/ShoppingCart
            routes.MapRoute(
                name: "ShoppingCart",
                url: "ShoppingCart",
                defaults: new { controller = "Order", action = "ShoppingCart" });

            //从购物车中移除选购商品：/ShoppingCart/Remove
            routes.MapRoute(
                name: "Remove",
                url: "ShoppingCart/Remove",
                defaults: new { controller = "Order", action = "Remove" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") });

            //结账支付：/CheckOut
            routes.MapRoute(
                name: "CheckOut",
                url: "CheckOut",
                defaults: new { controller = "Order", action = "CheckOut" });

            //用户登录：/Login
            routes.MapRoute(
                name: "Login",
                url: "Login",
                defaults: new { controller = "Account", action = "Login" });

            //登出注销：/Logout
            routes.MapRoute(
                name: "Logout",
                url: "Logout",
                defaults: new { controller = "Account", action = "Logout" });
        }
    }
}
