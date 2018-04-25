using Microsoft.Owin;
using Owin;

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