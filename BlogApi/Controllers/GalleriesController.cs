using BlogApi.Data;
using BlogApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[Route("api/[controller]")]
[ApiController]
public class GalleriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public GalleriesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Gallery>>> GetGalleries()
    {
        return await _context.gallery.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Gallery>> CreateGallery(Gallery gallery)
    {
        _context.gallery.Add(gallery);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetGalleries), new { id = gallery.id }, gallery);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGallery(int id, Gallery gallery)
    {
        if (id != gallery.id) return BadRequest();

        var existingGallery = await _context.gallery.FindAsync(id);
        if (existingGallery == null) return NotFound();

        existingGallery.image_url = gallery.image_url;
        existingGallery.alt_text = gallery.alt_text;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGallery(int id)
    {
        var gallery = await _context.gallery.FindAsync(id);
        if (gallery == null) return NotFound();

        _context.gallery.Remove(gallery);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage(IFormFile file, [FromForm] string altText)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Lütfen bir dosya seçin.");

        // Dosya uzantısını kontrol et
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(fileExtension))
            return BadRequest("Geçersiz dosya formatı. Yalnızca .jpg, .jpeg, .png, .gif uzantıları desteklenmektedir.");

        var fileName = Guid.NewGuid().ToString() + fileExtension;
        var filePath = Path.Combine("wwwroot/uploads", fileName);

        // Dosyayı kaydet
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Alt metin boşsa varsayılan değer ver
        var galleryItem = new Gallery
        {
            image_url = "/uploads/" + fileName,
            alt_text = string.IsNullOrWhiteSpace(altText) ? "Varsayılan açıklama" : altText
        };

        // Veritabanına kaydet
        _context.gallery.Add(galleryItem);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Görsel başarıyla yüklendi!", galleryItem });
    }



}
