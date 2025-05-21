using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BlogsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            return await _context.blogs.OrderByDescending(b => b.created_at).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Blog>> GetBlog(int id)
        {
            var blog = await _context.blogs.FindAsync(id);
            if (blog == null) return NotFound();
            return blog;
        }

        [HttpPost]
        public async Task<ActionResult<Blog>> CreateBlog(Blog blog)
        {
            _context.blogs.Add(blog);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBlog), new { id = blog.id }, blog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlog(int id, Blog blog)
        {
            if (id != blog.id) return BadRequest();

            var existingBlog = await _context.blogs.FindAsync(id);
            if (existingBlog == null) return NotFound();

            existingBlog.title = blog.title;
            existingBlog.summary = blog.summary;
            existingBlog.content = blog.content;
            existingBlog.image_url = blog.image_url;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.blogs.FindAsync(id);
            if (blog == null) return NotFound();

            _context.blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
