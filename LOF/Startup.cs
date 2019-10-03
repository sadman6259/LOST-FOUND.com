using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LOF.Startup))]
namespace LOF
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
