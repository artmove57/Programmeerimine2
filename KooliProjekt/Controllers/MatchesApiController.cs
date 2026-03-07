using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MatchesApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/MatchesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Matches>>> GetMatches()
        {
            return await _context.Matches
                .Include(m => m.Team)
                .Include(m => m.Tournament)
                .ToListAsync();
        }

        // GET: api/MatchesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Matches>> GetMatch(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Team)
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
            {
                return NotFound();
            }

            return match;
        }

        // POST: api/MatchesApi
        [HttpPost]
        public async Task<ActionResult<Matches>> PostMatch(Matches match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatch), new { id = match.Id }, match);
        }

        // PUT: api/MatchesApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMatch(int id, Matches match)
        {
            if (id != match.Id)
            {
                return BadRequest();
            }

            _context.Entry(match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/MatchesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/MatchesApi/teams - Get all teams for dropdown
        [HttpGet("teams")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams()
        {
            return await _context.Teams.ToListAsync();
        }

        // GET: api/MatchesApi/tournaments - Get all tournaments for dropdown
        [HttpGet("tournaments")]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetTournaments()
        {
            return await _context.Tournaments.ToListAsync();
        }

        private bool MatchExists(int id)
        {
            return _context.Matches.Any(e => e.Id == id);
        }
    }
}
