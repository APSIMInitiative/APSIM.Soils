using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(APSIM.Soils.Portal.Startup))]
namespace APSIM.Soils.Portal
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
