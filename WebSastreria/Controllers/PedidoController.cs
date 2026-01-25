using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sastreria_domain.entities;
using sastreria_domain.repositories;

namespace WebSastreria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly ICitaRepository _citaRepository;

        public PedidoController(IPedidoRepository pedidoRepository, ICitaRepository citaRepository)
        {
            _pedidoRepository = pedidoRepository;
            _citaRepository = citaRepository;
        }
        
        //-----------------------------------------------------------
        /*private async Task ActualizarEstadoPedido(int pedidoId)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido == null) return;
        
            var citas = await _citaRepository.GetByPedidoIdAsync(pedidoId);
        
            if (!citas.Any())
            {
                pedido.IdEstado = 1; // Pendiente
            }
            else if (citas.Any(c => c.Estado == false))
            {
                // false = realizada (según tu lógica actual)
                pedido.IdEstado = 2; // En proceso
            }
            else
            {
                pedido.IdEstado = 1; // Pendiente (solo hay citas futuras)
            }
        
            await _pedidoRepository.UpdateAsync(pedido.IdPedido, pedido);
        }*/
        //---------------------------------------------------------------

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var pedidos = await _pedidoRepository.GetAllAsync();
            return Ok(pedidos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null) return NotFound();
            return Ok(pedido);
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var pedidos = await _pedidoRepository.GetPagedAsync(page, pageSize);
        
            return Ok(pedidos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PedidoDomain pedidoDomain)
        {
            var created = await _pedidoRepository.CreateAsync(pedidoDomain);
            return Ok(created);
        }

        /*[HttpPost("{id}/cerrar")]
        public async Task<IActionResult> CerrarPedido(int id)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null) return NotFound();
        
            var citas = await _citaRepository.GetByPedidoIdAsync(id);
            if (citas.Any(c => c.Estado == true))
                return BadRequest("No se puede cerrar el pedido con citas pendientes.");
        
            pedido.IdEstado = 3; // Completado
            await _pedidoRepository.UpdateAsync(id, pedido);
        
            return Ok();
        }*/

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, int nuevoEstado)
        {
            var pedido = await _pedidoRepository.GetByIdAsync(id);
            if (pedido == null) return NotFound();
        
            pedido.IdEstado = nuevoEstado;
        
            await _pedidoRepository.UpdateAsync(pedido);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PedidoDomain pedidoDomain)
        {
            await _pedidoRepository.UpdateAsync(id, pedidoDomain);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pedidoRepository.DeleteAsync(id);
            return NoContent();
        }
    }

}
