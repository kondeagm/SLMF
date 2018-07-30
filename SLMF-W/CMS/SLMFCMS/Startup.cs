using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SLMFCMS.Startup))]
namespace SLMFCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
