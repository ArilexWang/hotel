using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(hotel_version1._0.Startup))]
namespace hotel_version1._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
