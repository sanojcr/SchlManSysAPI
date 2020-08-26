using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SchlManSysAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            // Values Route
            config.Routes.MapHttpRoute(
            name: "Values",
            routeTemplate: "api/values/{id}",
            defaults: new { controller = "school", id = RouteParameter.Optional }
            );

            // Student Route
            config.Routes.MapHttpRoute(
            name: "Students",
            routeTemplate: "api/students/{id}",
            defaults: new { controller = "school", id = RouteParameter.Optional }
            );

            // Default Route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
