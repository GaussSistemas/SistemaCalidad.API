using SistemaDeCalidad.API.Interfaces.Repositories;
using SistemaDeCalidad.API.Interfaces.Services;
using SistemaDeCalidad.API.Models;
using SistemaDeCalidad.API.Repositories;
using SistemaDeCalidad.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<SistemaDeCalidadConfiguration>(builder.Configuration.GetSection("SistemaDeCalidadConfiguration"));

builder.Services.AddCors(options =>
options.AddPolicy(name: "TecSer", builder => {
    builder.WithOrigins("https://develop.login.jalisco365.com.ar", "https://login.jalisco365.com.ar", "https://develop.dashboard.jalisco365.com.ar", "https://dashboard.jalisco365.com.ar", "https://develop.vendedores.jalisco365.com.ar", "https://vendedores.jalisco365.com.ar", "http://localhost:3000", "http://localhost:3005");
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
}));
builder.Services.AddAutoMapper(System.AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IEncuestasService, EncuestasService>();
builder.Services.AddTransient<ISoportesService, SoportesService>();
builder.Services.AddTransient<IEncuestasRepository, EncuestasRepository>();
builder.Services.AddTransient<ISoportesRepository, SoportesRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("TecSer");
app.Run();