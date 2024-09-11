using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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






List<Producto> productos = new List<Producto>();

// Endpoint para obtener todos los productos
app.MapGet("/productos", () =>
{
    return Results.Ok(productos);
});

// Endpoint para obtener un producto por Id
app.MapGet("/productos/{id:int}", (int id) =>
{
    var producto = productos.FirstOrDefault(p => p.Id == id);
    return producto is not null ? Results.Ok(producto) : Results.NotFound("Producto no encontrado");
});

// Endpoint para crear un nuevo producto
app.MapPost("/productos", (Producto producto) =>
{
    producto.Id = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;
    productos.Add(producto);
    return Results.Created($"/productos/{producto.Id}", producto);
});

// Endpoint para modificar un producto existente
app.MapPut("/productos/{id:int}", (int id, Producto productoActualizado) =>
{
    var productoExistente = productos.FirstOrDefault(p => p.Id == id);
    if (productoExistente is null)
    {
        return Results.NotFound("Producto no encontrado");
    }

    productoExistente.Nombre = productoActualizado.Nombre;
    productoExistente.Precio = productoActualizado.Precio;
    return Results.Ok(productoExistente);
});

// Endpoint para eliminar un producto
app.MapDelete("/productos/{id:int}", (int id) =>
{
    var producto = productos.FirstOrDefault(p => p.Id == id);
    if (producto is not null)
    {
        productos.Remove(producto);
        return Results.Ok("Producto eliminado");
    }
    return Results.NotFound("Producto no encontrado");
});

app.Run();

// Clase Producto
public class Producto
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public decimal Precio { get; set; }
}

