using ApiProject.BddTests.Setup;
using TechTalk.SpecFlow;

namespace ApiProject.BddTests.Steps
{

    [Binding]
    public sealed class TestServerSteps : CommonSteps
    {
        public TestServerSteps(Context context) : base(context)
        {
        }

        [Given(@"test server is setup")]
        public void GivenTestServerIsSetup()
        {
            Context.ApiProjectTestFixture = StaticHosts.ApiProjectTestFixture;
        }

    }

    [Binding]
    public class CommonSteps : TechTalk.SpecFlow.Steps
    {
        protected readonly Context Context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonSteps"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CommonSteps(Context context)
        {
            Context = context;
        }
    }

    public class Context
    {
        public ApiProjectTestFixture ApiProjectTestFixture { get; set; }
    }

    public static class StaticHosts
    {
        public static ApiProjectTestFixture ApiProjectTestFixture { get; set; }
    }
}