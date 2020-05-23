using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiProject.BddTests.Setup;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ApiProject.BddTests
{
    public class ValuesControllerTests
    {
        private ApiProjectTestFixture _fixture;
        private ApiProjectTestFixture _fixture2;

        [OneTimeSetUp]
        public void Setup()
        {
            _fixture = new ApiProjectTestFixture();
            //_fixture2 = new ApiProjectTestFixture();
        }

        [Test]
        public async Task TestGet()
        {
            var response = await _fixture.Client.GetAsync($"/api/values");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<string>>(json);

            Assert.NotNull(response);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("testvalue1", result.FirstOrDefault());
            Assert.AreEqual("testvalue2", result.LastOrDefault());
        }
    }
}
