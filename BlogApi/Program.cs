using BlogApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;  // JWT do�rulama i�in gerekli
using System.Text;  // Encoding s�n�f�n� kullanabilmek i�in
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);

// CORS yap�land�rmas� ekleyin
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();

// DbContext'i servis container'a ekleyin
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // PostgreSQL i�in UseNpgsql

// JWT do�rulama i�in gerekli ayarlar� ekleyin
var key = Encoding.ASCII.GetBytes("your_secret_key_here");  // Gizli anahtar�n�z� burada belirtin

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "your_issuer",  // Burada ge�erli issuer (yay�nc�) ad� belirtin
            ValidAudience = "your_audience",  // Burada ge�erli audience (hedef kitle) ad� belirtin
            IssuerSigningKey = new SymmetricSecurityKey(key)  // Burada anahtar�n�z� kullan�n
        };
    });

// Swagger yap�land�rmas�
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// CORS politikas�n� kullan�n
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseStaticFiles(); // wwwroot alt�ndaki statik dosyalara eri�im izni verir

app.UseRouting();
// HTTPS y�nlendirmesini kald�r�yoruz
// app.UseHttpsRedirection();  // Bunu kald�r�yoruz

// HTTP portunu belirleyelim
app.Urls.Add("http://localhost:7257");  // HTTP URL'yi ekleyin

app.UseAuthorization();
app.MapControllers();

app.Run();
