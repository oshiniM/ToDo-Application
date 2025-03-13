using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TodoApp.Data;
using TodoApp.Services;
using TodoApp.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserAuthService, UserAuthService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6; 
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<JwtTokenGenerator>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var jwtKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key is missing");
    var jwtIssuer = configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer is missing");
    var jwtAudience = configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience is missing");
    var jwtExpiry = int.TryParse(configuration["Jwt:ExpiryMinutes"], out int expiry) ? expiry : 60;

    return new JwtTokenGenerator(jwtKey, jwtIssuer, jwtAudience, jwtExpiry);
});


// JWT Authentication Configuration
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new ArgumentNullException("Jwt:Key is missing in appsettings.json");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new ArgumentNullException("Jwt:Issuer is missing in appsettings.json");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new ArgumentNullException("Jwt:Audience is missing in appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, 
            ValidateAudience = true, 
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
