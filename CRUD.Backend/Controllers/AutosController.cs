using CRUD.Backend.Data;
using CRUD.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutosController : ControllerBase
{
    private readonly DataContext _context;
    // Constructor that accepts a DataContext instance
    public AutosController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Consultar(string? filtro)
    {
        try
        {
            // Si no se envía filtro, devolvemos todo
            var query = _context.Autos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                filtro = filtro.ToLower();

                query = query.Where(a =>
                    (a.Marca + " " + a.Modelo + " " + a.Precio + " " + a.Year)
                    .ToLower()
                    .Contains(filtro)
                );
            }

            var autos = await query.ToListAsync();

            return Ok(new
            {
                Message = "Ok",
                Success = true,
                Data = autos
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Message = ex.Message,
                Success = false
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Crear(Auto entity)
    {
        if (entity == null)
            return BadRequest("Auto cannot be null");

        _context.Autos.Add(entity);
        await _context.SaveChangesAsync();

        // Devolver siempre el producto creado (con su Id)
        return Ok(entity);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Modificar(int id, Auto entity)
    {
        if (id != entity.Id)
        {
            return BadRequest("ID mismatch");
        }

        _context.Entry(entity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            return Ok(entity);
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
    public async Task<IActionResult> Eliminar(int id)
    {
        var entity = await _context.Autos.FindAsync(id);
        if (entity == null)
        {
            return NotFound();
        }

        _context.Autos.Remove(entity);
        try
        {
            await _context.SaveChangesAsync();
            return Ok(entity);
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
