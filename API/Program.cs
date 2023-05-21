using API.Extensions;
using API.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);  //services added to ApplicationServicesExtensions.cs to mage this class cleaner

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");  //middleware 51  not found endpoint now gives api response and not just an empty 404 not found

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();      //now API server knows it needs to serve static content not only HTTP requests 46

app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();       //using => disposed method is going to be called after finished
var servicces = scope.ServiceProvider;
var context =servicces.GetRequiredService<StoreContext>();
var logger = servicces.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
   logger.LogError(ex, "An error occured during migration");
}

app.Run();
