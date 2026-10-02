using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemaDeCalidad.API.Helpers.Logging;
using SistemaDeCalidad.API.Interfaces.Repositories;
using SistemaDeCalidad.API.Interfaces.Services;
using SistemaDeCalidad.API.Interfaces.Services.Bloqueo;
using SistemaDeCalidad.API.Models;
using SistemaDeCalidad.API.Persistence.Context;
using SistemaDeCalidad.API.Repositories;
using SistemaDeCalidad.API.Services;
using SistemaDeCalidad.API.Services.Bloqueo;

var builder = WebApplication.CreateBuilder(args);

// Log a archivo (por defecto C:\Logs, configurable con "FileLogging:Carpeta").
builder.Logging.AddProvider(new FileLoggerProvider(builder.Configuration["FileLogging:Carpeta"] ?? @"C:\Logs"));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] and then your valid JWT token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    // 2. Require the scheme globally (applies to all endpoints)
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
builder.Services.Configure<SistemaDeCalidadConfiguration>(builder.Configuration.GetSection("SistemaDeCalidadConfiguration"));
builder.Services.Configure<JWTConfiguration>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<CRMConfiguration>(builder.Configuration.GetSection("CRMConfiguration"));
builder.Services.AddDbContext<SistemaDeCalidadContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("SistemaDeCalidadDB")));

builder.Services.AddCors(options =>
options.AddPolicy(name: "TecSer", builder =>
{
    builder.WithOrigins("https://develop.login.jalisco365.com.ar", "https://login.jalisco365.com.ar", "https://develop.dashboard.jalisco365.com.ar", "https://dashboard.jalisco365.com.ar", "https://develop.vendedores.jalisco365.com.ar", "https://vendedores.jalisco365.com.ar", "http://localhost:3000", "http://localhost:3005");
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
}));
builder.Services.AddAutoMapper(System.AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IEncuestasService, EncuestasService>();
builder.Services.AddTransient<ISoportesService, SoportesService>();
builder.Services.AddTransient<IEncuestasRepository, EncuestasRepository>();
builder.Services.AddTransient<ISoportesRepository, SoportesRepository>();
builder.Services.AddTransient<IUsersService, UsersService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IStepsService, StepsService>();
builder.Services.AddTransient<IMessagesTypesService, MessagesTypesService>();
builder.Services.AddTransient<IMessagesService, MessagesService>();

// Dual record de encuestas hacia el CRM.
var crmConfiguration = builder.Configuration.GetSection("CRMConfiguration").Get<CRMConfiguration>() ?? new CRMConfiguration();
builder.Services.AddHttpClient(CRMEncuestasService.HttpClientName, client =>
{
    client.Timeout = TimeSpan.FromSeconds(crmConfiguration.TimeoutSegundos > 0 ? crmConfiguration.TimeoutSegundos : 8);
});
builder.Services.AddTransient<ICRMEncuestasService, CRMEncuestasService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.MapInboundClaims = false;
    options.UseSecurityTokenValidators = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                context.Response.Headers.Add("Token-Expired", "true");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Deja asentado en el log con qué configuración del CRM arrancó la API,
// para poder diagnosticar el dual record en el servidor publicado.
app.Logger.LogInformation(
    "API iniciada. Entorno {Entorno}. CRM Habilitado {Habilitado} URL '{Url}' Token configurado {TieneToken} (largo {LargoToken}, solo ASCII {TokenAscii})",
    app.Environment.EnvironmentName, crmConfiguration.Habilitado, crmConfiguration.IngestaURL,
    !string.IsNullOrWhiteSpace(crmConfiguration.Token),
    crmConfiguration.Token?.Trim().Length ?? 0, crmConfiguration.Token?.Trim().All(char.IsAscii) ?? true);

// Si al publicar se pisa el appsettings.json del servidor, faltan estas claves
// y todos los requests fallan. Mejor dejarlo explícito en el log.
foreach (var clave in new[] { "Jwt:Key", "Jwt:Issuer", "Jwt:Audience", "ConnectionStrings:SistemaDeCalidadDB" })
{
    if (string.IsNullOrWhiteSpace(builder.Configuration[clave]))
        app.Logger.LogCritical("Falta la configuración '{Clave}' (revisar el appsettings.json del servidor). La API no va a funcionar correctamente.", clave);
}

var swaggerEnabled = builder.Configuration.GetValue<bool?>("Swagger:Enabled") ?? false;

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || swaggerEnabled)
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseHttpsRedirection();
app.UseCors("TecSer");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();