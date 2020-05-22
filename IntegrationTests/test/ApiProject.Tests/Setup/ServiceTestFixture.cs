using ApiProject.BusinessLogic;
using ApiProject.Tests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ApiProject.Tests.Setup
{
    public class ServiceTestFixture : TestFixture<Startup>
    {
        protected override void ConfigureServices(WebHostBuilderContext hostingContext, IServiceCollection services)
        {
            Bootstrap.Container.Options.AllowOverridingRegistrations = true;
        }

        protected override void ConfigureTestServices(IServiceCollection services)
        {
            Bootstrap.Container.Register<IValuesBusinessLogic, FakeValuesBusinessLogic>();

        }
    }
}