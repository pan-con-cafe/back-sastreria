using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sastreria_data.repositories;
using sastreria_domain.entities;
using sastreria_domain.repositories;

namespace WebSastreria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class CitaController : ControllerBase
    {
        private readonly ICitaRepository _citaRepository;
        private readonly ICitaImagenRepository _citaImagenRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IPedidoRepository _pedidoRepository;

        public CitaController(
            ICitaRepository citaRepository,
            ICitaImagenRepository citaImagenRepository,
            IClienteRepository clienteRepository, 
            IPedidoRepository pedidoRepository)
        {
            _citaRepository = citaRepository;
            _citaImagenRepository = citaImagenRepository;
            _clienteRepository = clienteRepository;
            _pedidoRepository = pedidoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var citas = await _citaRepository.GetAllAsync();
            return Ok(citas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null) return NotFound();
            return Ok(cita);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CitaDomain citaDomain)
        {
            // Siempre crea un nuevo cliente
            var cliente = await _clienteRepository.CreateAsync(new ClienteDomain
            {
                IdTipoDocumento = 1,
                NumeroDocumento = citaDomain.Cliente.NumeroDocumento,
                Nombre = citaDomain.Cliente.Nombre,
                Apellido = citaDomain.Cliente.Apellido,
                Correo = citaDomain.Cliente.Correo,
                Telefono = citaDomain.Cliente.Telefono
            });

            // Crear pedido asociado
            var pedido = new PedidoDomain
            {
                IdCliente = cliente.IdCliente,
                IdEstado = 2, // En Proceso
                Detalle = null,
                FechaEntrega = null,
                IdModelo = citaDomain.PedidoId ?? null, // Usa PedidoId si llega desde el front
                IdSastre = null
            };
            pedido = await _pedidoRepository.CreateAsync(pedido);

            // Crear cita y asociar el pedido recién creado
            var nuevaCita = new CitaDomain
            {
                IdCliente = cliente.IdCliente,
                FechaCita = citaDomain.FechaCita,
                PedidoId = pedido.IdPedido, // Asocia el pedido creado
                Estado = true,
                Notas = ""
            };

            await _citaRepository.CreateAsync(nuevaCita);

            return Ok(new
            {
                message = "Cita y pedido creados correctamente",
                cita = nuevaCita,
                pedido = pedido
            });
        }


        [HttpGet("{idCita}/imagenes")]
        public async Task<IActionResult> GetImagenesPorCita(int idCita)
        {
            var imagenes = await _citaImagenRepository.GetByCitaAsync(idCita);
            if (imagenes == null || !imagenes.Any())
            {
                return NotFound();
            }
            return Ok(imagenes);
        }

        [HttpPost("{idCita}/imagenes")]
        public async Task<IActionResult> AgregarImagenes(int idCita, [FromBody] List<string> urls)
        {
            foreach (var url in urls)
            {
                var citaImagen = new CitaImagenDomain
                {
                    IdCita = idCita,
                    Url = url
                };

                await _citaImagenRepository.CreateAsync(citaImagen);
            }

            return Ok(new { message = "Imágenes agregadas correctamente." });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CitaDomain citaDomain)
        {
            await _citaRepository.UpdateAsync(id, citaDomain);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _citaRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
