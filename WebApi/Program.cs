using Domain.JsonConfigs;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using UserManagement.Infrastructure;
using UserManagement.Service;
using Serilog;
using Infrastructure.Extensions;
using Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var section = builder.Configuration.GetSection("ConnectionStrings");
builder.Services.Configure<ConnectionStrings>(section);

builder.Services.AddSingleton<DbConn>();
builder.AddInfra();
builder.AddPresentation();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IRabbitMqService, RabbitMqService>();

builder.Services.AddDbContext<LocalDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB"));
});

builder.Host.UseSerilog((context, services, loggerConfig) =>
    loggerConfig.ReadFrom.Configuration(context.Configuration)
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseEFCoreMigrations();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
