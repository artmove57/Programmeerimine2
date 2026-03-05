using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.Controllers
{
    [Route("api/Teams")]
    [ApiController]
    public class TeamsApiController : ControllerBase
    {
        private readonly ITeamService _service;

        public TeamsApiController(ITeamService service)
        {
            _service = service;
        }

        // GET: api/Teams
        [HttpGet]
        public async Task<IEnumerable<Team>> Get()
        {
            var result = await _service.List(1, 10000, null);
            return result.Results;
        }

        // GET api/Teams/5
        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            var team = await _service.Get(id);
            if (team == null)
            {
                return NotFound();
            }

            return team;
        }

        // POST api/Teams
        [HttpPost]
        public async Task<object> Post([FromBody] Team team)
        {
            await _service.Save(team);

            return Ok(team);
        }

        // PUT api/Teams/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Team team)
        {
            if (id != team.Id)
            {
                return BadRequest();
            }

            await _service.Save(team);

            return Ok();
        }

        // DELETE api/Teams/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _service.Get(id);
            if (team == null)
            {
                return NotFound();
            }

            await _service.Delete(id);

            return Ok();
        }
    }
}
