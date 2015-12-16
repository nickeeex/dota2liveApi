using dota2liveApi.Code;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Mvc;

namespace dota2liveApi
{
    public class WebApiConfig
    {

        public static void Register(HttpConfiguration config)
        {
            //Hosting backend and frontend on different domains so cors is required. 
            //Client is implemented in the same visual studio project, for now, so this is not needed but this enables other websites to consume the same api 
            config.EnableCors();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Force usage of https
            config.Filters.Add(new ForceHttpsAttribute());

            //Add json as the default return type
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

    }
}
