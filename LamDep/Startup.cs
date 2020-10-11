using Owin;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(LamDep.Startup))]
namespace LamDep
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
