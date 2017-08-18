using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;

namespace LangAppServer
{
    public static class WebApiConfig
    {
        public const string DEFAULT_ROUTE_NAME = "DefaultApi";
        public static void Register(HttpConfiguration config)
        {
           // config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: DEFAULT_ROUTE_NAME,
                routeTemplate: "api/{controller}/{action}/{uid}",
                defaults: new { uid = RouteParameter.Optional }
                ); 
            config.EnableSystemDiagnosticsTracing();
            config.Formatters.JsonFormatter.SupportedMediaTypes
    .Add(new MediaTypeHeaderValue("text/html"));
        }






        /*
        // Web API configuration and services
        // Configure Web API to use only bearer token authentication.
        config.SuppressDefaultHostAuthentication();
        config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
        config.EnableSystemDiagnosticsTracing();
        // Web API routes
        config.MapHttpAttributeRoutes();

        config.Routes.MapHttpRoute(
            name: DEFAULT_ROUTE_NAME,
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional }

        );*/
    }
}

