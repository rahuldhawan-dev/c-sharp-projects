using ApiProject.BusinessLogic;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculatorBusinessLogic _logic;

        public CalculatorController(ICalculatorBusinessLogic logic)
        {
            _logic = logic;
        }

        // GET api/values/5
        [HttpGet("{a}/{b}")]
        public ActionResult<int> Get(int a, int b)
        {
            return _logic.Sum(a, b);
        }
    }
}
