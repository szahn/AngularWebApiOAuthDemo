using System.Web.Http;
using Demo.Api;
using Demo.Auth.OAuth;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Demo.Api
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            /*Note: the order in which the app is initialized DOES matter. */
            var config = new HttpConfiguration();
            app.UseOAuth();
            app.UseWebApi(config);
            WebApiConfig.Register(config);
        }


    }
}
