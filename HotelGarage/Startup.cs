using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HotelGarage.Startup))]
namespace HotelGarage
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
