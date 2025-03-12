using Carter;
using Catalog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);


//Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();
app.MapCarter();

// Configure the HTTP request pipeline.

app.Run();
