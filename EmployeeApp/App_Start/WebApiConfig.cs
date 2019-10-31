using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EmployeeApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Filters.Add(new basicAuthenticationAttributes());
            //// Web API routes
            config.MapHttpAttributeRoutes();
            // Controllers with Actions
            // To handle routes like `/api/User/Login`
            config.Routes.MapHttpRoute(
                name: "ControllerAndAction",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
   
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
        }
    }
}
