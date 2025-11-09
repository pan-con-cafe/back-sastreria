using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sastreria_domain.entities;
using sastreria_domain.repositories;
using sastreria_domain.services;
using WebSastreria.Dtos;

namespace WebSastreria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IReniecService _reniecService;
        public ClienteController(IClienteRepository clienteRepository, IReniecService reniecService)
        {
            _clienteRepository = clienteRepository;
            _reniecService = reniecService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clientes = await _clienteRepository.GetAllAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var cliente = await _clienteRepository.GetByIdAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(ClienteCreateDto clienteDto)
        {
            try
            {
                // Validar correo formato
                if (!string.IsNullOrEmpty(clienteDto.Correo))
                {
                    var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                    if (!emailRegex.IsMatch(clienteDto.Correo))
                        return BadRequest("El correo no tiene un formato válido.");
                }

                string? nombres = clienteDto.Nombres;
                string? apellidoPaterno = clienteDto.ApellidoPaterno;
                string? apellidoMaterno = clienteDto.ApellidoMaterno;

                if (clienteDto.IdTipoDocumento == 1) // 1 = DNI
                {
                    if (string.IsNullOrEmpty(clienteDto.NumeroDocumento) || clienteDto.NumeroDocumento.Length != 8 || !clienteDto.NumeroDocumento.All(char.IsDigit))
                        return BadRequest("El DNI debe tener exactamente 8 dígitos.");

                    var datosReniec = await _reniecService.ObtenerDatosPorDniAsync(clienteDto.NumeroDocumento);
                    if (datosReniec == null)
                        return BadRequest("DNI no encontrado en RENIEC.");

                    nombres = datosReniec.Nombres;
                    apellidoPaterno = datosReniec.ApellidoPaterno;
                    apellidoMaterno = datosReniec.ApellidoMaterno;
                }
                else if (clienteDto.IdTipoDocumento == 2) // 2 = Pasaporte
                {
                    if (string.IsNullOrEmpty(clienteDto.NumeroDocumento) || clienteDto.NumeroDocumento.Length < 6)
                        return BadRequest("El Pasaporte debe tener al menos 6 caracteres.");

                    if (string.IsNullOrWhiteSpace(clienteDto.Nombres) ||
                        string.IsNullOrWhiteSpace(clienteDto.ApellidoPaterno) ||
                        string.IsNullOrWhiteSpace(clienteDto.ApellidoMaterno))
                    {
                        return BadRequest("Nombres, Apellido Paterno y Apellido Materno son obligatorios para Pasaporte.");
                    }
                }

                var clienteDomain = new ClienteDomain
                {
                    IdTipoDocumento = clienteDto.IdTipoDocumento,
                    NumeroDocumento = clienteDto.NumeroDocumento,
                    Correo = clienteDto.Correo,
                    Telefono = clienteDto.Telefono,
                    Nombre = nombres ?? "",
                    Apellido = $"{apellidoPaterno} {apellidoMaterno}".Trim()
                };

                var created = await _clienteRepository.CreateAsync(clienteDomain);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al crear el cliente: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        [HttpGet("buscar-por-dni/dni/{dni}")]
        public async Task<IActionResult> BuscarPorDni(string dni)
        {
            var datosReniec = await _reniecService.ObtenerDatosPorDniAsync(dni);
            if (datosReniec == null)
                return NotFound();

            return Ok(new
            {
                nombres = datosReniec.Nombres,
                apellidos = $"{datosReniec.ApellidoPaterno} {datosReniec.ApellidoMaterno}".Trim()
            });
        }

        [HttpGet("por-dni/{dni}")]
        public async Task<IActionResult> GetByDni(string dni)
        {
            var cliente = await _clienteRepository.GetByNumeroDocumentoAsync(dni);
            if (cliente == null)
                return NotFound();

            return Ok(cliente);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteDomain clienteDomain)
        {
            try
            {
                await _clienteRepository.UpdateAsync(id, clienteDomain);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar el cliente: {ex.InnerException?.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteRepository.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al eliminar el cliente: {ex.InnerException?.Message}");
            }
        }
    }
}
