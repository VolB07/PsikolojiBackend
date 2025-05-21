using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContactsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
    {
        return await _context.contact_info.ToListAsync();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, Contact contact)
    {
        if (id != contact.id) return BadRequest();

        var existingContact = await _context.contact_info.FindAsync(id);
        if (existingContact == null) return NotFound();

        existingContact.address = contact.address;
        existingContact.whatsapp = contact.whatsapp;
        existingContact.email = contact.email;

        await _context.SaveChangesAsync();
        return NoContent();
    }
}
