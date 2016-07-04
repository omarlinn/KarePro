using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Karepro.Startup))]
namespace Karepro
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
