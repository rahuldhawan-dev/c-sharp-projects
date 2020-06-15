using System;
using System.Threading;
using ApiProject.BddTests.Setup;
using ApiProject.BddTests.Steps;
using BoDi;
using TechTalk.SpecFlow;

namespace ApiProject.BddTests
{
	[Binding]
	public sealed class Hooks
	{
		// For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

		public Hooks(IObjectContainer container)
        {
            container.RegisterInstanceAs<ITest>(new Test() { Id = "ctor"});
        }

		static Hooks()
		{
		}

		[BeforeScenario]
		public void BeforeScenario()
		{
			//empty for test purposes
		}

		[AfterScenario]
		public void AfterScenario()
		{
			//empty for test purposes
		}

		[BeforeTestRun]
		public static void BeforeTestRun(IObjectContainer container)
		{
            container.RegisterInstanceAs<ITest>(new Test() { Id = "BeforeTestRun" });
			Console.WriteLine("Setting up BDD Tests: deleting test data and rebuilding lookups...");
            StaticHosts.ApiProjectTestFixture = new ApiProjectTestFixture();
		}

        [AfterTestRun]
		public static void AfterTestRun()
		{
			//DashboardHost.Dispose();
			//AuthHost.Dispose();
			Thread.Sleep(100);  // We need to allow the web app and test threads to terminate before exiting the test
		}
	}

    public interface ITest
    {
        string Id { get; set; }
    }

	public class Test: ITest
	{
        public string Id { get; set; }
    }
}
