using ApiProject.BddTests.Fakes;
using ApiProject.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;
using TestUtilities.Setup;

namespace ApiProject.BddTests.Setup
{
    public class ServiceTestFixture : TestFixture<Startup>
    {
        protected override void ConfigureTestServices(IServiceCollection services)
        {
            Bootstrap.Container.Options.AllowOverridingRegistrations = true;
            Bootstrap.Container.Register<IValuesBusinessLogic, FakeValuesBusinessLogic>();
            Bootstrap.Container.Register<ICalculatorBusinessLogic, FakeCalculatorBusinessLogic>();

        }
    }
}