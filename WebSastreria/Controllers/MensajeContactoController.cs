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
    private readonly AppDbContext _context;

    public MensajeContactoController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/MensajeContacto
    [HttpPost]
    public async Task<IActionResult> EnviarMensaje([FromBody] MensajeContacto mensaje)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        mensaje.Fecha = DateTime.Now;
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
}
