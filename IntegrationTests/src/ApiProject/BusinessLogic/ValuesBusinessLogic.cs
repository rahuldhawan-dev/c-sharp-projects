using System.Collections.Generic;

namespace ApiProject.BusinessLogic
{
    public class ValuesBusinessLogic : IValuesBusinessLogic
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }

    public interface IValuesBusinessLogic
    {
        IEnumerable<string> Get();
    }
}
