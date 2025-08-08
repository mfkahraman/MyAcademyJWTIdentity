using JWTIdentity.API.Context;
using JWTIdentity.API.Entities;
using JWTIdentity.API.Options;
using JWTIdentity.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//JwtService registration
builder.Services.AddScoped<IJwtService, JwtService>();

// JwtSecurityTokenHandler registration
builder.Services.AddSingleton<JwtSecurityTokenHandler>();

builder.Services.AddAuthentication(cfg =>
{
    cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt=>
{
    var jwtTokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>()
    ?? throw new InvalidOperationException("TokenOptions configuration is missing or invalid.");

    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtTokenOptions.Issuer,
        ValidAudience = jwtTokenOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOptions.Key ?? throw new InvalidOperationException("TokenOptions.Key is missing."))),
        ClockSkew = TimeSpan.Zero,
        NameClaimType = ClaimTypes.Name,
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Default", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<TokenOptions>(builder.Configuration.GetSection(nameof(TokenOptions)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
