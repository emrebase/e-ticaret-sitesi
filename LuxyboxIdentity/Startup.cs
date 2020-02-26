using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LuxyboxIdentity.Startup))]
namespace LuxyboxIdentity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
