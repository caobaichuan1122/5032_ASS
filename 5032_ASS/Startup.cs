using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(_5032_ASS.Startup))]
namespace _5032_ASS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
