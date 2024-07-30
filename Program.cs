using System.Text;
using Backend.Data;
using Backend.Services.Books;
using Microsoft.EntityFrameworkCore;
using Backend.Services.Mailersend;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json.Serialization;
using Backend.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
// Aprende más sobre la configuración de Swagger/OpenAPI en https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); // Añade soporte para exploración y documentación de API con Swagger.
builder.Services.AddSwaggerGen(); // Registra el generador de Swagger para la documentación de la API.
builder.Services.AddHttpClient(); // Añade soporte para servicios de cliente HTTP.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configura las opciones de JSON para la aplicación.
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; // Habilita el manejo de referencias para preservar las referencias de objeto en la serialización JSON.
    });

// Configura el DbContext para la conexión con la base de datos MySQL.
builder.Services.AddDbContext<BaseContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MySqlConnection"), // Recupera la cadena de conexión desde la configuración.
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.20-mysql"))); // Especifica la versión del servidor para MySQL.

builder.Services.AddScoped<IBookRepository, BookRepository>(); // Registra BookRepository para la inyección de dependencias.
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Registra UserRepository para la inyección de dependencias.
builder.Services.AddScoped<IMailersendServices, MailersendServices>(); // Registra MailersendServices para la inyección de dependencias.

var app = builder.Build(); // Construye la aplicación.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilita Swagger en el entorno de desarrollo.
    app.UseSwaggerUI(); // Configura la interfaz de usuario de Swagger para la documentación de la API.
}

app.UseHttpsRedirection(); // Fuerza la redirección a HTTPS.
app.MapControllers(); // Mapea los endpoints de los controladores.

app.Run(); // Ejecuta la aplicación.
