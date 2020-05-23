using ApiProject.BusinessLogic;
using ApiProject.Tests.Fakes;
using Microsoft.Extensions.DependencyInjection;
using TestUtilities.Setup;

namespace ApiProject.Tests.Setup
{
    public class ServiceTestFixture : TestFixture<Startup>
    {
        protected override void ConfigureTestServices(IServiceCollection services)
        {
            Bootstrap.Container.Options.AllowOverridingRegistrations = true;
            Bootstrap.Container.Register<IValuesBusinessLogic, FakeValuesBusinessLogic>();
        }
    }
}