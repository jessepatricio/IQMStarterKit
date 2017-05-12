using IQMStarterKit.Models;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
