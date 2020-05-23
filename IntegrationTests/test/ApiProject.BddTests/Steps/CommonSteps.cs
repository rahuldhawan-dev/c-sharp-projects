using ApiProject.BddTests.Setup;
using TechTalk.SpecFlow;

namespace ApiProject.BddTests.Steps
{

    [Binding]
    public sealed class LoginSteps : CommonSteps
    {
        public LoginSteps(Context context) : base(context)
        {
        }

        [Given(@"test server is setup")]
        public void GivenTestServerIsSetup()
        {
            Context.ApiProjectTestFixture = new ApiProjectTestFixture();
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
            //if (context.m_aliases == null)
            //    Context.m_aliases = new Dictionary<string, string>();
            //ConfigProvider = Bootstrap.Container.GetInstance<IConfigProvider>();
            //WorkflowDataProvider = Bootstrap.Container.GetInstance<IWorkflowDataProvider>();
        }

    }

    public class Context
    {
        //public ValuesTestFixture ValuesTestFixture { get; set; }

        public ApiProjectTestFixture ApiProjectTestFixture { get; set; }
    }
}