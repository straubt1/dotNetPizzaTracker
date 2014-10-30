using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Microsoft.Owin.Cors;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;

namespace PizzaTracker
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
         //   config.EnableCors(new EnableCorsAttribute("*", "*", "GET, POST, OPTIONS, PUT, DELETE"));
            //config.EnableCors();
           
            // Web API configuration and services
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{format}",
                defaults: new { id = RouteParameter.Optional, format = RouteParameter.Optional }
            );
        }

        //public static void RegisterFormatters(MediaTypeFormatterCollection formatters)
        //{
        //    var jsonFormatter = formatters.JsonFormatter;
        //    jsonFormatter.SerializerSettings = new JsonSerializerSettings
        //    {
        //        ContractResolver = new CamelCasePropertyNamesContractResolver()
        //    };

        //    // Insert the JSONP formatter in front of the standard JSON formatter.
        //    var jsonpFormatter = new JsonpMediaTypeFormatter(formatters.JsonFormatter);
        //    formatters.Insert(0, jsonpFormatter);
        //}
    }
}
