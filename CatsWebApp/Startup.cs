using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CatsWebApp.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]
namespace CatsWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
