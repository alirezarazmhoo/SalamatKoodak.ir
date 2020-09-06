using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SalamatKoodak.Startup))]
namespace SalamatKoodak
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
