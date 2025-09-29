using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ImageCompress_asp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("index","","~/image_page.aspx"); // for default route page
            routes.MapPageRoute("error", "{*url}", "~/error.aspx"); // error page
        }
    }
}
