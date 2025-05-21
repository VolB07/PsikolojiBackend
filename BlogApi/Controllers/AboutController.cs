using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class AboutController : ControllerBase
{
    private readonly AppDbContext _context;

    public AboutController(AppDbContext context)
    {
        _context = context;
    }

    // Get method to retrieve all About entries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<About>>> GetAbout()
    {
        return await _context.about.ToListAsync();
    }

    // PUT method to update an existing About entry
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAbout(int id, About about)
    {
        if (id != about.id) return BadRequest();

        var existingAbout = await _context.about.FindAsync(id);
        if (existingAbout == null) return NotFound();

        // Güncelleme işlemi
        existingAbout.name = about.name;
        existingAbout.university = about.university;
        existingAbout.experience = about.experience;
        existingAbout.description = about.description;
        existingAbout.image_url = about.image_url;

        await _context.SaveChangesAsync();
        return NoContent();
    }

}
