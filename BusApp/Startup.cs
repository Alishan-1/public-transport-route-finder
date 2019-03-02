using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BusApp.Startup))]
namespace BusApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
