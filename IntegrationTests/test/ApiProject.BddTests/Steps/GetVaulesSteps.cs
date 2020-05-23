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
    public class GetVaulesSteps: CommonSteps
    {
        private readonly Context _context;

        public GetVaulesSteps(Context context) : base(context)
        {
            _context = context;
        }

        [Given(@"nothing")]
        public void GivenNothing()
        {
            
        }

        [When(@"get values multiple is called")]
        public void WhenGetValuesMultipleIsCalled()
        {
            
        }

        [Then(@"multiple values are returned")]
        public async Task ThenMultipleValuesAreReturned()
        {
            var response = await _context.ApiProjectTestFixture.Client.GetAsync($"/api/values");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<string>>(json);

            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("testvalue1", result.FirstOrDefault());
            Assert.AreEqual("testvalue2", result.LastOrDefault());
        }
    }
}