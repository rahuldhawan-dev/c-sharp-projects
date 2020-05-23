using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace ApiProject.BddTests.Steps
{

    [Binding]
    public class MathSteps: CommonSteps
    {
        private readonly Context _context;
        private int _a;
        private int _b;

        public MathSteps(Context context) : base(context)
        {
            _context = context;
        }

        [Given(@"I have entered (.*) into the calculator as a")]
        public void GivenIHaveEnteredIntoTheCalculatorAsA(int a)
        {
            _a = a;
        }

        [Given(@"I have entered (.*) into the calculator as b")]
        public void GivenIHaveEnteredIntoTheCalculatorAsB(int b)
        {
            _b = b;
        }

        [When(@"I press add")]
        public void WhenIPressAdd()
        {
            
        }

        [Then(@"the result should be (.*) on the screen")]
        public async Task ThenTheResultShouldBeOnTheScreen(int sum)
        {
            var response = await _context.ApiProjectTestFixture.Client.GetAsync($"/api/calculator/{_a}/{_b}");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<int>(json);

            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(sum, result);
        }

    }
}