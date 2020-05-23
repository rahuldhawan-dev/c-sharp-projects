using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ApiProject.BddTests.Setup
{
    public class ApiProjectTestFixture : ServiceTestFixture
    {
        protected override void ConfigureServices(WebHostBuilderContext hostingContext, IServiceCollection services)
        {
            base.ConfigureServices(hostingContext, services);
        }
    }
}