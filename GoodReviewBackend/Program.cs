using GoodReviewBackend.Models;
using GoodReviewBackend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Rejestracja serwisów
builder.Services.AddScoped<IAuthService, AuthService>(); // Rejestracja AuthService
builder.Services.AddScoped<PasswordHasherService>();   // Rejestracja PasswordHasherService
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<KsiazkiUzytkownikaService>();
builder.Services.AddScoped<IGatunkowoscService, GatunkowoscService>();


// Dodajemy konfiguracjê JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // Wartoœæ dla Issuera
            ValidAudience = builder.Configuration["Jwt:Audience"], // Wartoœæ dla Audience
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Klucz
        };
    });

// Rejestracja DbContext
builder.Services.AddDbContext<GoodReviewDatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // ConnectionString z konfiguracji

// Rejestracja pozosta³ych us³ug
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Konfiguracja CORS (zezwolenie na dostêp z aplikacji Angular)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Adres aplikacji Angular
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Uruchomienie procesu aktualizacji hase³ w ramach cyklu ¿ycia aplikacji (scoped)


// Middleware dla Swaggera w trybie deweloperskim
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp"); // W³¹czenie CORS

app.UseAuthentication();  // Middleware dla autentykacji JWT
app.UseAuthorization();   // Middleware dla autoryzacji

app.MapControllers(); // Mapowanie kontrolerów

app.Run();
