using TechTalk.SpecFlow;

namespace ApiProject.BddTests.Steps
{

    [Binding]
    public class MathSteps: CommonSteps
    {
        public MathSteps(Context context) : base(context)
        {
        }

        [Given(@"I have entered (.*) into the calculator as a")]
        public void GivenIHaveEnteredIntoTheCalculatorAsA(int a)
        {
            
        }

        [Given(@"I have entered (.*) into the calculator as b")]
        public void GivenIHaveEnteredIntoTheCalculatorAsB(int b)
        {
            
        }

        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            
        }

        [Then(@"the result should be (.*) on the screen")]
        public void ThenTheResultShouldBeOnTheScreen(int p0)
        {
            
        }

    }
}