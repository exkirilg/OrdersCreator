using DataAccess;
using Domain.DataAccess;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1.0.0", new OpenApiInfo
    {
        Version = "v1.0.0",
        Title = "OrdersCreator",
        Contact = new OpenApiContact
        {
            Email = builder.Configuration["Contacts:Email"]
        },
        License = new OpenApiLicense
        {
            Name = "MIT Licence"
        }
    });

    options.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory,
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("OrdersCreator"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IProvidersServices, ProvidersServices>();
builder.Services.AddScoped<IOrdersServices, OrdersServices>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("v1.0.0/swagger.json", "OrdersCreator API v1.0.0");
});

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
