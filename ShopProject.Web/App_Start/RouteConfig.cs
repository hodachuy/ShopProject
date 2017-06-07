using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopProject.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
                name: "Confirm Order",
                url: "xac-nhan-don-hang.html",
                defaults: new { controller = "ShoppingCart", action = "ConfirmOrder", id = UrlParameter.Optional },
                namespaces: new string[] { "ShopProject.Web.Controllers" }
               );
            routes.MapRoute(
               name: "Cancel Order",
               url: "huy-don-hang.html",
               defaults: new { controller = "ShoppingCart", action = "CancelOrder", id = UrlParameter.Optional },
               namespaces: new string[] { "ShopProject.Web.Controllers" }
              );

            routes.MapRoute(
                 name: "Contact",
                 url: "lien-he.html",
                 defaults: new { controller = "Contact", action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "ShopProject.Web.Controllers" }
                );
            routes.MapRoute(
                  name: "Search",
                  url: "tim-kiem.html",
                  defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
                  namespaces: new string[] { "ShopProject.Web.Controllers" }
              );
            routes.MapRoute(
                 name: "Login",
                 url: "dang-nhap.html",
                 defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                 namespaces: new string[] { "ShopProject.Web.Controllers" }
             );
            routes.MapRoute(
               name: "Register",
               url: "dang-ky.html",
               defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
               namespaces: new string[] { "ShopProject.Web.Controllers" }
           );
            routes.MapRoute(
            name: "Cart",
            url: "gio-hang.html",
            defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
            namespaces: new string[] { "ShopProject.Web.Controllers" }
        );
            routes.MapRoute(
            name: "ViewCart",
            url: "view-cart.html",
            defaults: new { controller = "ShoppingCart", action = "ViewCart", id = UrlParameter.Optional },
            namespaces: new string[] { "ShopProject.Web.Controllers" }
        );
            routes.MapRoute(
           name: "CountCart",
           url: "count-cart.html",
           defaults: new { controller = "ShoppingCart", action = "CountCart", id = UrlParameter.Optional },
           namespaces: new string[] { "ShopProject.Web.Controllers" }
       );
            routes.MapRoute(
           name: "Checkout",
           url: "thanh-toan.html",
           defaults: new { controller = "ShoppingCart", action = "CheckOut", id = UrlParameter.Optional },
           namespaces: new string[] { "ShopProject.Web.Controllers" }
       );
            routes.MapRoute(
                   name: "Page",
                   url: "trang/{alias}.html",
                   defaults: new { controller = "Page", action = "Checkout", alias = UrlParameter.Optional },
                   namespaces: new string[] { "ShopProject.Web.Controllers" }
               );

            routes.MapRoute(
                 name: "Product Category",
                 url: "{alias}.pc-{id}.html",
                 defaults: new { controller = "Product", action = "ProductsByCategoryID", id = UrlParameter.Optional },
                   namespaces: new string[] { "ShopProject.Web.Controllers" }
             );

             routes.MapRoute(
                 name: "Groups",
                 url: "{alias}.gr-{id}.html",
                 defaults: new { controller = "Product", action = "ProductsByGroup", id = UrlParameter.Optional },
                   namespaces: new string[] { "ShopProject.Web.Controllers" }
             );

            routes.MapRoute(
                 name: "Product",
                 url: "{alias}.p-{productId}.html",
                 defaults: new { controller = "Product", action = "Detail", productId = UrlParameter.Optional },
                   namespaces: new string[] { "ShopProject.Web.Controllers" }
             );
            routes.MapRoute(
                 name: "TagList",
                 url: "tag/{tagId}.html",
                 defaults: new { controller = "Product", action = "ListByTag", tagId = UrlParameter.Optional },
                   namespaces: new string[] { "ShopProject.Web.Controllers" }
             );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
