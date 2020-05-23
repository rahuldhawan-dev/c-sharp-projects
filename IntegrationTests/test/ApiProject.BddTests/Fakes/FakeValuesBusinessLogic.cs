using System.Collections.Generic;
using ApiProject.BusinessLogic;

namespace ApiProject.Tests.Fakes
{
    public class FakeValuesBusinessLogic : IValuesBusinessLogic
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "testvalue1", "testvalue2" };
        }

        public string Get(int id)
        {
            return $"testvalue{id}";
        }
    }
}