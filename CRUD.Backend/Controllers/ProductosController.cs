using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers;

// Este controlador expone la API en la ruta "api/productos".
// El atributo [Route("api/[controller]")] toma el nombre de la clase (ProductosController)
// y lo convierte en parte de la URL (quitando "Controller").
// De esta forma, el frontend puede comunicarse con este backend usando rutas como:
// - GET    api/productos        -> obtener todos los productos
// - GET    api/productos/{id}   -> obtener un producto por ID
// - POST   api/productos        -> crear un nuevo producto
// - PUT    api/productos/{id}   -> actualizar un producto existente
// - DELETE api/productos/{id}   -> eliminar un producto por ID
[Route("api/[controller]")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly DataContext _context;
    // Constructor that accepts a DataContext instance
    public ProductosController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var productos = await _context.Productos.ToListAsync();
        return Ok(productos);
    }

    
    [HttpPost]
    public async Task<IActionResult> PostAsync(Producto producto)
    {
        if (producto == null)
            return BadRequest("Producto cannot be null");

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        // Devolver siempre el producto creado (con su Id)
        return Ok(producto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound();
        }
        return Ok(producto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAsync(int id, Producto producto)
    {
        if (id != producto.Id)
        {
            return BadRequest("ID mismatch");
        }

        _context.Entry(producto).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(producto);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException!.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound();
        }

        _context.Productos.Remove(producto);
        try
        {
            await _context.SaveChangesAsync();
            return Ok(producto);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException!.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
