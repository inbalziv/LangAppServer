using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LangAppServer
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("","api/{controller}/{action}/{uid}");
        }
    }
}
/*
 *  routes.MapPageRoute("",
        "Category/{action}/{categoryName}",
        "~/categoriespage.aspx",
        true,
        new RouteValueDictionary 
            {{"categoryName", "food"}, {"action", "show"}});
            /getdata/{uid:string}



    name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          
 * */
