using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ApiProject.Tests.Setup
{
    public class ValuesTestFixture : ServiceTestFixture
    {
        protected override void ConfigureServices(WebHostBuilderContext hostingContext, IServiceCollection services)
        {
            base.ConfigureServices(hostingContext, services);
        }
    }
}