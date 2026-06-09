using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oracle.EntityFrameworkCore;
using System.Text;
using Void.API.Data;
using Void.API.Middlewares;
using Void.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurando o Banco de Dados Oracle
builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseOracle(
        builder.Configuration.GetConnectionString("OracleFIAP"),
        oracleOptions =>
        {
            oracleOptions.UseOracleSQLCompatibility(
                OracleSQLCompatibility.DatabaseVersion21
            );
        });
});

// Registrando o TokenService na Injeção de Dependência
builder.Services.AddScoped<TokenService>();

// Configuração de Autenticação JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.ASCII.GetBytes(jwtSettings.GetValue<string>("SecretKey"));

builder.Services.AddAuthentication(options =>
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
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidateAudience = true,
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        ValidateLifetime = true
    };
});

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configurando o Swagger para exigir Token JWT
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VOID API - Reabilitação Biométrica",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Cole APENAS o seu token JWT aqui."
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

// Middleware Global de Tratamento de Erros
app.UseMiddleware<GlobalExceptionMiddleware>();

// Swagger habilitado
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilita o CORS antes da autenticação
app.UseCors("PermitirFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();