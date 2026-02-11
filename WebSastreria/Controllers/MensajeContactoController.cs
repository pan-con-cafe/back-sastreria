using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sastreria_data.database.tables;
using sastreria_data.repositories;
using sastreria_domain.Errors;
using sastreria_domain.repositories;
using sastreria_domain.RequestResponse;
using WebSastreria.models;

[ApiController]
[Route("api/[controller]")]
public class MensajeContactoController : ControllerBase
{
    private readonly _dbContext _context;

    public MensajeContactoController(_dbContext context)
    {
        _context = context;
    }

    // POST: api/MensajeContacto
    [HttpPost]
    public async Task<IActionResult> EnviarMensaje([FromBody] MensajeContacto mensaje)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        mensaje.Fecha = DateTime.UtcNow;
        mensaje.Leido = false;

        _context.MensajeContactos.Add(mensaje);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Mensaje enviado correctamente" });
    }

    // GET (opcional, para el admin)
    [HttpGet]
    public async Task<IActionResult> ObtenerMensajes()
    {
        var mensajes = await _context.MensajeContactos
            .OrderByDescending(m => m.Fecha)
            .ToListAsync();

        return Ok(mensajes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMensaje(int id)
    {
        var mensaje = await _context.MensajeContacto.FindAsync(id);
        if (mensaje == null) return NotFound();

        return Ok(mensaje);
    }

    [HttpPut("{id}/leido")]
    public async Task<IActionResult> MarcarComoLeido(int id)
    {
        var mensaje = await _context.MensajeContacto.FindAsync(id);
        if (mensaje == null) return NotFound();

        mensaje.Leido = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMensaje(int id)
    {
        var mensaje = await _context.MensajeContacto.FindAsync(id);
        if (mensaje == null) return NotFound();
    
        _context.Mensajes.Remove(mensaje);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }


}
