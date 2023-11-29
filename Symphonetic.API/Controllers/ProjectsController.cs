using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Symphonetic.API.Controllers;

using Data;
using Models;

public class ProjectsController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : ControllerBase
{
    
    // POST: api/Projects
    [HttpPost]
    public async Task<ActionResult<Project>> CreateProject([FromBody] Project project)
    {
        // Business logic and validation here
        // ...

        dbContext.Projects.Add(project);
        await dbContext.SaveChangesAsync();

        return CreatedAtAction("GetProject", new { id = project.Id }, project);
    }
    
    // GET: api/Projects/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Project>> GetProject(Guid id)
    {
        var project = await dbContext.Projects.Include(p => p.Tickets).FirstOrDefaultAsync(p => p.Id == id);

        if (project == null)
        {
            return NotFound();
        }

        return project;
    }
    
    // PUT: api/Projects/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] Project project)
    {
        if (id != project.Id)
        {
            return BadRequest();
        }

        dbContext.Entry(project).State = EntityState.Modified;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!dbContext.Projects.Any(e => e.Id == id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }
    
    // DELETE: api/Projects/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        var project = await dbContext.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        dbContext.Projects.Remove(project);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }
    
}
