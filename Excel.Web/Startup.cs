using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Excel.Web.Startup))]
namespace Excel.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
