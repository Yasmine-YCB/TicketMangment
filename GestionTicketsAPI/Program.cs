using System.Text.Json;
using GestionTicketsAPI.Extensions;
using GestionTicketsAPI.Middleware;
 
using Hangfire;
using Hangfire.MySql;
using OfficeOpenXml;


var builder = WebApplication.CreateBuilder(args);

  
 
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Configuration des CORS
var allowedOrigins = new string[]
{
    "http://localhost:8040"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});


var app = builder.Build();

app.MapGet("/", () => "Bienvenue dans l'API GestionTicketsAPI !");

// Middleware d'exception
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🚀 **Place UseCors ici, avant Authentication et Authorization**
app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
