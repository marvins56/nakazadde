using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(nakazadde.Startup))]
namespace nakazadde
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
