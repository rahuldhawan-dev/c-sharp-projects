using System;

namespace ApiProject.BusinessLogic
{
    public interface ICalculatorBusinessLogic
    {
        int Sum(int a, int b);
    }

    public class CalculatorBusinessLogic : ICalculatorBusinessLogic
    {
        public int Sum(int a, int b)
        {
            throw new NotImplementedException();
            //return a + b;
        }
    }
}