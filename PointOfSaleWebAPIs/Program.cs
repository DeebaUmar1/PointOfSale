using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PointOfSale;
using PointOfSaleWebAPIs;
using PointOfSaleWebAPIs.Middlewares;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddLog4Net();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TokenService>(); // Register TokenService
builder.Services.AddDbContext<POSDbContext>(options =>
    options.UseInMemoryDatabase("POSDatabase"));

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<POSDbContext>();
    POSDbContext.SeedData(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//app.UseMiddleware<BasicAuthMiddleware>();
app.UseMiddleware<BearerTokenMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
