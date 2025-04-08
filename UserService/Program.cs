using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var jwtKey = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]);
var issuer = builder.Configuration["Jwt:Issuer"];
var audience = builder.Configuration["Jwt:Audience"];
var validadeAudience = true;
if (builder.Environment.IsDevelopment())
{
    validadeAudience = false; ;
}
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = issuer, // Gateway

            ValidateAudience = validadeAudience,
            ValidAudience = audience, // Gateway

            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
               jwtKey
            ),
            ValidateLifetime = true
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Service", Version = "v1" });

    // Configuração para o Swagger suportar autenticação JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira seu token JWT no campo abaixo (sem 'Bearer ')"
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
            new string[] {}
        }
    });
});




var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.Use(async (context, next) =>
{
    var allowedOrigin = "https://localhost:7080"; // Apenas o Gateway pode chamar
    var referer = context.Request.Headers["Referer"].ToString();
    var origin = context.Request.Headers["Origin"].ToString();

    if (!string.IsNullOrEmpty(origin) && origin != allowedOrigin &&
        !string.IsNullOrEmpty(referer) && !referer.StartsWith(allowedOrigin))
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Acesso negado: chame através do API Gateway.");
        return;
    }

    await next();
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();



app.MapControllers();
app.MapGet("/users", [Authorize] () => new[] { new { Id = 1, Name = "Alice" }, new { Id = 2, Name = "Bob" } });

app.Run();
