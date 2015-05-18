using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(zavrsni.Startup))]
namespace zavrsni
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
