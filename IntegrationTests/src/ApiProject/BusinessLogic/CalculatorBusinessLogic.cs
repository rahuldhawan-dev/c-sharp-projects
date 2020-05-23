namespace ApiProject.Controllers
{
    public interface ICalculatorBusinessLogic
    {
        int Sum(int a, int b);
    }

    public class CalculatorBusinessLogic : ICalculatorBusinessLogic
    {
        public int Sum(int a, int b)
        {
            return a + b;
        }
    }
}