using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartupAttribute(typeof(Web.Startup))]
namespace Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Register Web API routing support before anything else
            //GlobalConfiguration.Configure(WebApiConfig.Register);

            ConfigureAuth(app);
        }
    }
}