using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PEDFAM.Startup))]
namespace PEDFAM
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
