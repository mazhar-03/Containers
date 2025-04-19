using Containers.Application;
using Containers.Models;

var builder = WebApplication.CreateBuilder(args);
var ConnectionString = builder.Configuration.GetConnectionString("UniversityDatabase");

builder.Services.AddTransient<IContainerServiceRepository, ContainerService>(
    _ => new ContainerService(ConnectionString));
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/containers", (IContainerServiceRepository service) =>
{
    try
    {
        return Results.Ok(service.GetAllContainers());
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapPost("/api/containers", (IContainerServiceRepository service, Container container) =>
{
    try
    {
        var success = service.Create(container);

        if (success) return Results.Created();
        return Results.BadRequest();
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.MapDelete("/api/containers/{id}", (IContainerServiceRepository service, int id) =>
{
    bool success = service.Delete(id);
    return success ? Results.NoContent() : Results.StatusCode(500);
});



app.Run();