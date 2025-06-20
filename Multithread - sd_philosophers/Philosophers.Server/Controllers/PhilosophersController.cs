using Microsoft.AspNetCore.Mvc;

namespace Philosophers.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhilosophersController : ControllerBase
    {
        private static readonly DiningPhilosophers _diningPhilosophers = new(5);

        [HttpPost("start/{id}")]
        public async Task<IActionResult> Start(int id)
        {
            await _diningPhilosophers.StartDiningAsync(id);
            return Ok();
        }

        [HttpGet("states")]
        public IActionResult GetStates()
        {
            return Ok(_diningPhilosophers.GetStates());
        }
    }
}
