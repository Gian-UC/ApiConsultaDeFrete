using EnvioRapidoApi.Repositories;
using EnvioRapidoApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using EnvioRapidoApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ✅ Adiciona appsettings e variáveis de ambiente
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

// ======================================================================
// 🔒 Função para ler segredos de forma segura
static string ReadSecret(string filePath, string envVar, IConfiguration config, string configKey)
{
    // 1) Se veio de arquivo montado via Docker secrets
    if (File.Exists(filePath))
        return File.ReadAllText(filePath).Trim();

    // 2) Se veio de variável de ambiente
    var fromEnv = Environment.GetEnvironmentVariable(envVar);
    if (!string.IsNullOrWhiteSpace(fromEnv))
        return fromEnv.Trim();

    // 3) Se veio do appsettings / secrets development
    var fromConfig = config[configKey];
    if (!string.IsNullOrWhiteSpace(fromConfig))
        return fromConfig.Trim();

    return string.Empty;
}
// ======================================================================

// ✅ Carregar JWT Key com fallback seguro
var jwtKeyRaw = ReadSecret("/run/secrets/jwt_key", "Jwt__Key", builder.Configuration, "Jwt:Key");
if (string.IsNullOrWhiteSpace(jwtKeyRaw))
    throw new Exception("JWT Key não configurada!");

var jwtKeyBytes = Encoding.UTF8.GetBytes(jwtKeyRaw);
if (jwtKeyBytes.Length < 32)
    throw new Exception("JWT Key precisa ter 256 bits (mínimo 32 bytes).");

// ✅ Carregar Token do Melhor Envio
var melhorEnvioToken = ReadSecret("/run/secrets/melhorenvio_token", "MelhorEnvio__Token", builder.Configuration, "MelhorEnvio:Token");
if (string.IsNullOrWhiteSpace(melhorEnvioToken))
    throw new Exception("Token do Melhor Envio não configurado!");

// ✅ Torna token visível para injection normal
builder.Configuration["MelhorEnvio:Token"] = melhorEnvioToken;

// ✅ MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ✅ Swagger + Bearer JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Envio Rápido API",
        Version = "v1",
        Description = "API para cálculo e envio de fretes com MelhorEnvio + RabbitMQ + MySQL"
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu_token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

// ✅ Autenticação JWT
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
        IssuerSigningKey = new SymmetricSecurityKey(jwtKeyBytes),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


// ✅ Injeção de dependência
builder.Services.AddScoped<EnvioRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<RabbitMqService>();
builder.Services.AddScoped<AuthService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient<ViaCepService>();
builder.Services.AddHttpClient<MelhorEnvioService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
