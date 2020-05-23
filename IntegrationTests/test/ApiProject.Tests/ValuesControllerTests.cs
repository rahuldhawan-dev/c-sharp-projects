using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ApiProject.Tests.Setup;
using Newtonsoft.Json;
using NUnit.Framework;

namespace ApiProject.Tests
{
    public class ValuesControllerTests
    {
        private ValuesTestFixture _fixture;
        
        [OneTimeSetUp]
        public void Setup()
        {
            _fixture = new ValuesTestFixture();
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

        [Test]
        public async Task TestGet2()
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
