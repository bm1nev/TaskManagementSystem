using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Infrastructure.Auth;
using TaskManagementSystem.Infrastructure.Persistence;
using TaskManagementSystem.Infrastructure.Repositories;
using TaskManagementSystem.Infrastructure.Seeding;

var builder = WebApplication.CreateBuilder(args);

// Controllers (ще ни трябват за AuthController следващата стъпка)
builder.Services.AddControllers();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// JWT options
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>()!;

// DI: repositories + services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher, AspNetPasswordHasher>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserAdminService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ProjectService>();

// AuthN/AuthZ
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,

            ValidateAudience = true,
            ValidAudience = jwt.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey))
        };
    });

builder.Services.AddAuthorization();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TaskManagementSystem API",
        Version = "v1"
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {   
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter: Bearer { token }"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Migrate + Seed (Development only)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.Migrate();
    await DbSeeder.SeedAsync(db);

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
