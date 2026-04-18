using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SesaAPI.Data.Context;
using SesaAPI.Logic.Repositories;
using SesaAPI.Logic.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<SesaDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Sesa API",
        Description = "API para la gestión de pólizas de seguro de vehículos"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.EnableAnnotations();
}

);

builder.Services.AddCustomerRepository();
builder.Services.AddCoverageRepository();
builder.Services.AddVehicleRepository();
builder.Services.AddVehicleService();
builder.Services.AddPolicyRepository();
builder.Services.AddPolicyService();

var app = builder.Build();

/* Esto lo usé para ejecutar las migraciones y crear las tablas de la base de datos. Lo dejo comentado ya que para facilitar la revisión
 * de esta prueba mejor les agregué un script para que no necesiten hacer nada más antes de ejecutar el proyecto*/
/*using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SesaDBContext>();
    dbContext.Database.Migrate();
}*/

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
