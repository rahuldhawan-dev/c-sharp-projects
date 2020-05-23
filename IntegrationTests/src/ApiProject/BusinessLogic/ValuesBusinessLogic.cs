using System.Collections.Generic;

namespace ApiProject.BusinessLogic
{
    public class ValuesBusinessLogic : IValuesBusinessLogic
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        public string Get(int id)
        {
            return $"value{id}";
        }
    }

    public interface IValuesBusinessLogic
    {
        IEnumerable<string> Get();
        string Get(int id);
    }
}
