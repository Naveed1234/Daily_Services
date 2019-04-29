using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Daily_Services.Startup))]
namespace Daily_Services
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
