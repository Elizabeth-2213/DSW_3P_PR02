using DSW_3P_PR02.Data;
using Microsoft.EntityFrameworkCore;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException(
            "Falta configurar ConnectionStrings:DefaultConnection.")
    ));

builder.Services.AddScoped<IEventoRepository, EventoSQLRepository>();
builder.Services.AddScoped<IEventoService, EventoService>();

var app = builder.Build();
app.UseExceptionHandler();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Gestor de Eventos Tecnológicos v1");
        options.DocumentTitle = "Gestor de Eventos - Swagger";
    });
}
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
app.MapControllers();
app.Run();
