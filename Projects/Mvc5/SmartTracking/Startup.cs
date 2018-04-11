using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SmartTracking.Startup))]
namespace SmartTracking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
