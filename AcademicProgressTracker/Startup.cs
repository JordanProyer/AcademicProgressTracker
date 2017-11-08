using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AcademicProgressTracker.Startup))]
namespace AcademicProgressTracker
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
