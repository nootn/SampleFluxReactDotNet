using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SampleFluxReactDotNet.Web.Startup))]
namespace SampleFluxReactDotNet.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
