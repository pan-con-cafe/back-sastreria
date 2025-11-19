//using Azure.Core;
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
        private readonly IHorarioRepository _horarioRepository;
        private readonly IModeloRepository _modeloRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<CitaController> _logger;

        public CitaController(
            ICitaRepository citaRepository,
            ICitaImagenRepository citaImagenRepository,
            IClienteRepository clienteRepository,
            IEmailService emailService,
            IPedidoRepository pedidoRepository,
            IModeloRepository modeloRepository,
            IHorarioRepository horarioRepository,
            ILogger<CitaController> logger)
        {
            _citaRepository = citaRepository;
            _citaImagenRepository = citaImagenRepository;
            _clienteRepository = clienteRepository;
            _pedidoRepository = pedidoRepository;
            _horarioRepository = horarioRepository;
            _modeloRepository = modeloRepository;
            _emailService = emailService;
            _logger = logger;
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
                IdEstado = 1, // Pendiente
                Detalle = null,
                FechaEntrega = null,
                IdModelo = citaDomain.PedidoId ?? null, // Usa PedidoId si llega desde el front
                IdSastre = null
            };
            pedido = await _pedidoRepository.CreateAsync(pedido);

            // OBTENER HORARIO Y CALCULAR FECHA REAL
            if (!citaDomain.IdHorario.HasValue)
                return BadRequest("Debe seleccionar un horario.");

            var horario = await _horarioRepository.GetByIdAsync(citaDomain.IdHorario.Value);
            if (horario == null)
                return BadRequest("Horario no válido.");

            // Obtener fecha del próximo día que coincida
            DateTime fechaBase = ObtenerFechaReal(horario.Dia);

            // Combinar fecha + hora
            DateTime fechaCita = fechaBase.Date + horario.HoraInicio.ToTimeSpan();

            // Crear cita y asociar el pedido recién creado
            var nuevaCita = new CitaDomain
            {
                IdCliente = cliente.IdCliente,
                FechaCita = fechaCita,
                PedidoId = pedido.IdPedido, // Asocia el pedido creado
                Estado = true,
                Notas = ""
            };

            await _citaRepository.CreateAsync(nuevaCita);

            // ✅ OBTENER INFO DEL HORARIO Y MODELO PARA EL CORREO
            string horarioTexto = $"{horario.Dia} de {horario.HoraInicio:hh\\:mm} a {horario.HoraFin:hh\\:mm}";
            string nombreModelo = "Modelo personalizado";

            //if (citaDomain.IdHorario.HasValue)
            //{
            //    var horario = await _horarioRepository.GetByIdAsync(citaDomain.IdHorario.Value);
            //    if (horario != null)
            //    {
            //        horarioTexto = $"{horario.Dia} de {horario.HoraInicio:hh\\:mm} a {horario.HoraFin:hh\\:mm}";
            //    }
            //}

            if (pedido.IdModelo.HasValue)
            {
                var modelo = await _modeloRepository.GetByIdAsync(pedido.IdModelo.Value);
                if (modelo != null)
                {
                    nombreModelo = modelo.Nombre;
                }
            }

            // ✅ ENVIAR CORREO
            try
            {
                string nombreCompleto = $"{cliente.Nombre} {cliente.Apellido}";

                await _emailService.EnviarCorreoReservaAsync(
                    cliente.Correo,
                    nombreCompleto,
                    fechaCita,
                    horarioTexto,
                    nombreModelo
                );

                _logger.LogInformation($"Correo enviado a {cliente.Correo}");
            }
            catch (Exception emailEx)
            {
                _logger.LogWarning($"Error al enviar correo: {emailEx.Message}");
            }

            return Ok(new
            {
                message = "Cita y pedido creados correctamente",
                cita = nuevaCita,
                pedido = pedido
            });
        }

        // HELPER PARA OBTENER PRÓXIMA FECHA SEGÚN DÍA
        private DateTime ObtenerFechaReal(string diaSemana)
        {
            var hoy = DateTime.Today;

            var dias = new Dictionary<string, DayOfWeek>(StringComparer.OrdinalIgnoreCase)
            {
                { "Lunes", DayOfWeek.Monday },
                { "Martes", DayOfWeek.Tuesday },
                { "Miércoles", DayOfWeek.Wednesday },
                { "Miercoles", DayOfWeek.Wednesday },
                { "Jueves", DayOfWeek.Thursday },
                { "Viernes", DayOfWeek.Friday },
                { "Sábado", DayOfWeek.Saturday },
                { "Sabado", DayOfWeek.Saturday }
            };

            if (!dias.ContainsKey(diaSemana))
                throw new Exception($"Día inválido: {diaSemana}");

            var diaObjetivo = dias[diaSemana];

            int diferencia = ((int)diaObjetivo - (int)hoy.DayOfWeek + 7) % 7;

            if (diferencia == 0)
                diferencia = 7; // Si es hoy, tomamos la próxima semana

            return hoy.AddDays(diferencia);
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
