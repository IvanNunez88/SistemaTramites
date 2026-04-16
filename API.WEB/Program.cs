using API.WEB.Common;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();
builder.Services.AddFluentValidationConfig();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar DbContext (conexion a BD)
builder.Services.AddSingleton<API.WEB.Data.DbContext>();

// Registrar Services
builder.Services.AddScoped<API.WEB.Features.Alumnos.AlumnoService>();

// CORS - para que el FRONT pueda conectarse
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "SistemaTramites"));
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
