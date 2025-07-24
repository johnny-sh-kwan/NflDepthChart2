using Application.Extensions;
using Microsoft.OpenApi.Models;
using WebApi.Extensions;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    // Optional: Configure Swagger document info (title, version, etc.)
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Nfl Depth Chart Api",
        Description = "Nfl Depth Chart Api - Swagger"
    });
});

// register services from the Application project
builder.Services.AddApiServices();
builder.Services.AddApplicationServices();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

Endpoints.MapEndpoints(app);

app.Run();

