using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab1.Data;
using Lab1.Models;
using Microsoft.AspNetCore.Authorization;


// I, Jordan Webber, student number 000803303, certify that this material is my
// original work. No other person's work has been used without due
// acknowledgement and I have not made my work available to anyone else.
namespace Lab1.Controllers
{
    /// <summary>
    /// The Teams controller that can only be accessed by users with roles of player or manager.
    /// </summary>
    [Authorize(Roles = "Manager,Player")]
    public class TeamsController : Controller
    {
        /// <summary>
        /// The database context.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor for the teams controller.
        /// </summary>
        /// <param name="context">Database context</param>
        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Teams
        /// <summary>
        /// Gets the index page for the teams
        /// </summary>
        /// <returns>View of list of teams</returns>
        public async Task<IActionResult> Index()
        {
              return View(await _context.Teams.ToListAsync());
        }

        // GET: Teams/Details/5
        /// <summary>
        /// Gets the details for a specific team.
        /// </summary>
        /// <param name="id">id of the team</param>
        /// <returns>Page with team information</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        /// <summary>
        /// Gets the create teams page.
        /// </summary>
        /// <returns>Create team page</returns>
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Create([Bind("Id,TeamName,Email,EstablishedDate")] Team team)
        {
            if (ModelState.IsValid)
            {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Edit/5
        /// <summary>
        /// Gets the edit page for a team
        /// </summary>
        /// <param name="id">id of the team</param>
        /// <returns>edit page</returns>
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /// <summary>
        /// Updates the edited team
        /// </summary>
        /// <param name="id">Id of the team</param>
        /// <param name="team">Team object</param>
        /// <returns>Edit page for team</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamName,Email,EstablishedDate")] Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Delete/5
        /// <summary>
        /// Gets the delete page for a team.
        /// </summary>
        /// <param name="id">id of team</param>
        /// <returns>Page to delete team</returns>
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        /// <summary>
        /// Confirms deletion of a team.
        /// </summary>
        /// <param name="id">id of team</param>
        /// <returns>Page to delete team</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teams == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Teams'  is null.");
            }
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
          return _context.Teams.Any(e => e.Id == id);
        }
    }
}
