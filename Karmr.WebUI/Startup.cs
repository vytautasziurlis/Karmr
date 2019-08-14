using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Karmr.WebUI.Startup))]
namespace Karmr.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
