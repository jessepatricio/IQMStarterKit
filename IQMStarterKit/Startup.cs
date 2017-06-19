using Microsoft.Owin;
using Owin;


[assembly: OwinStartupAttribute(typeof(IQMStarterKit.Startup))]
namespace IQMStarterKit
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            
        }

        
    }
}
