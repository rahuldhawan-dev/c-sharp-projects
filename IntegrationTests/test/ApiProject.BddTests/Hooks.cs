using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using TechTalk.SpecFlow;

namespace ApiProject.BddTests
{
	[Binding]
	public sealed class Hooks
	{		
        //private static readonly ValuesTestFixture _fixture;
		private static readonly IWebHost AuthHost;
        //private static readonly DashboardHost DashboardHost;		
		// For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

		static Hooks()
		{
            
			//_fixture = new ValuesTestFixture();
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
		public static void BeforeTestRun()
		{
			Console.WriteLine("Setting up BDD Tests: deleting test data and rebuilding lookups...");
		}

		[AfterTestRun]
		public static void AfterTestRun()
		{
			//DashboardHost.Dispose();
			//AuthHost.Dispose();
			Thread.Sleep(100);  // We need to allow the web app and test threads to terminate before exiting the test
		}
	}
}
