using Microsoft.EntityFrameworkCore;
using Mosaico.Api.Infrastructure.Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Mosaico.Api.Settings;
using Microsoft.OpenApi.Models;
using Mosaico.Api.Middlewares;

using Mosaico.Api.Application.Interfaces;
using Mosaico.Api.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// ----------------------------
// JWT CONFIG
// ----------------------------
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);

var jwtSettings = jwtSection.Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            ClockSkew = TimeSpan.Zero
        };
    });

// ----------------------------
// CONTROLLERS
// ----------------------------
builder.Services.AddControllers();

// ----------------------------
// SWAGGER + JWT
// ----------------------------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MOSAICO+ Core API",
        Version = "v1",
        Description = "API do projeto MOSAICO+ (Global Solution – O Futuro do Trabalho)"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT como: Bearer {token}"
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

// ----------------------------
// DATABASE
// ----------------------------
builder.Services.AddDbContext<MosaicoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ----------------------------
// SERVICES (SOA)
// ----------------------------
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IMissionService, MissionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBadgeService, BadgeService>();

// ----------------------------
// BUILD
// ----------------------------
var app = builder.Build();

// ----------------------------
// MIDDLEWARE GLOBAL DE EXCEÇÕES
// ----------------------------
app.UseMiddleware<ExceptionHandlingMiddleware>();

// ----------------------------
// SWAGGER PIPELINE
// ----------------------------
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
