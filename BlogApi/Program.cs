using BlogApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;  // JWT doðrulama için gerekli
using System.Text;  // Encoding sýnýfýný kullanabilmek için
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);

// CORS yapýlandýrmasý ekleyin
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
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))); // PostgreSQL için UseNpgsql

// JWT doðrulama için gerekli ayarlarý ekleyin
var key = Encoding.ASCII.GetBytes("your_secret_key_here");  // Gizli anahtarýnýzý burada belirtin

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = "your_issuer",  // Burada geçerli issuer (yayýncý) adý belirtin
            ValidAudience = "your_audience",  // Burada geçerli audience (hedef kitle) adý belirtin
            IssuerSigningKey = new SymmetricSecurityKey(key)  // Burada anahtarýnýzý kullanýn
        };
    });

// Swagger yapýlandýrmasý
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// CORS politikasýný kullanýn
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseStaticFiles(); // wwwroot altýndaki statik dosyalara eriþim izni verir

app.UseRouting();
// HTTPS yönlendirmesini kaldýrýyoruz
// app.UseHttpsRedirection();  // Bunu kaldýrýyoruz

// HTTP portunu belirleyelim
app.Urls.Add("http://localhost:7257");  // HTTP URL'yi ekleyin

app.UseAuthorization();
app.MapControllers();

app.Run();
