//using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sastreria_data.repositories;
using sastreria_domain.entities;
using sastreria_domain.repositories;
using sastreria_domain.RequestResponse;

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
                IdModelo = citaDomain.IdModelo,
                IdSastre = null
            };
            pedido = await _pedidoRepository.CreateAsync(pedido);

            // OBTENER HORARIO Y CALCULAR FECHA REAL
            if (!citaDomain.IdHorario.HasValue)
                return BadRequest("Debe seleccionar un horario.");

            var horario = await _horarioRepository.GetByIdAsync(citaDomain.IdHorario.Value);
            if (horario == null)
                return BadRequest("Horario no válido.");

            if (horario.Estado != true)
                return BadRequest("Horario no disponible."); // 🔥

            // Obtener fecha del próximo día que coincida
            DateTime fechaBase = ObtenerFechaReal(horario.Dia);

            // Combinar fecha + hora
            DateTime fechaCita = fechaBase.Date + horario.HoraInicio.ToTimeSpan();

            // Crear cita y asociar el pedido recién creado
            var nuevaCita = new CitaDomain
            {
                IdCliente = cliente.IdCliente,
                PedidoId = pedido.IdPedido,
                IdHorario = horario.IdHorario,
                FechaCita = fechaCita,
                Estado = true,
                Notas = ""
            };

            await _citaRepository.CreateAsync(nuevaCita);

            horario.Estado = false;
            await _horarioRepository.UpdateAsync(horario.IdHorario, horario);

            // ✅ OBTENER INFO DEL HORARIO Y MODELO PARA EL CORREO
            string horarioTexto = $"{horario.Dia} de {horario.HoraInicio:hh\\:mm} a {horario.HoraFin:hh\\:mm}";
            string nombreModelo = "Modelo personalizado";


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

        [HttpGet("por-pedido/{pedidoId}")]
        public async Task<IActionResult> GetByPedidoId(int pedidoId)
        {
            var citas = await _citaRepository.GetByPedidoIdAsync(pedidoId);
        
            if (citas == null || !citas.Any())
                return Ok(new List<object>());
        
            return Ok(citas.Select(c => new
            {
                idCita = c.IdCita,
                idCliente = c.IdCliente,
                pedidoId = c.PedidoId,
                fechaCita = c.FechaCita,
                estado = c.Estado,    
                notas = c.Notas,
                idHorario = c.IdHorario
            }));
        }


        [HttpPost("desde-pedido")]
        public async Task<IActionResult> CrearDesdePedido(
        [FromBody] CrearCitaDesdePedidoRequest req)
        {
            // 1️⃣ Validaciones básicas
            if (req.IdCliente <= 0)
                return BadRequest("IdCliente inválido.");

            if (req.IdPedido <= 0)
                return BadRequest("IdPedido inválido.");

            if (req.IdHorario <= 0)
                return BadRequest("IdHorario inválido.");

            // 2️⃣ Validar cliente existente
            var cliente = await _clienteRepository.GetByIdAsync(req.IdCliente);
            if (cliente == null)
                return BadRequest("Cliente no existe.");

            // 3️⃣ Validar pedido existente
            var pedido = await _pedidoRepository.GetByIdAsync(req.IdPedido);
            if (pedido == null)
                return BadRequest("Pedido no existe.");

            // 4️⃣ Obtener horario
            var horario = await _horarioRepository.GetByIdAsync(req.IdHorario);
            if (horario == null)
                return BadRequest("Horario no válido.");

            if (horario.Estado != true)
                return BadRequest("Horario no disponible.");

            // 5️⃣ Calcular fecha real de la cita
            DateTime fechaBase = ObtenerFechaReal(horario.Dia);
            DateTime fechaCita = fechaBase.Date + horario.HoraInicio.ToTimeSpan();

            // 6️⃣ Crear cita
            var nuevaCita = new CitaDomain
            {
                IdCliente = cliente.IdCliente,
                PedidoId = pedido.IdPedido,
                IdHorario = horario.IdHorario,
                FechaCita = fechaCita,
                Estado = true,
                Notas = ""
            };

            await _citaRepository.CreateAsync(nuevaCita);

            horario.Estado = false;
            await _horarioRepository.UpdateAsync(horario.IdHorario, horario);
            //------------------------------------------
            //await ActualizarEstadoPedido(pedido.IdPedido);
            //------------------------------------------

            // 7️⃣ Datos para correo
            string horarioTexto =
                $"{horario.Dia} de {horario.HoraInicio:hh\\:mm} a {horario.HoraFin:hh\\:mm}";

            string nombreModelo = "Modelo personalizado";

            if (pedido.IdModelo.HasValue)
            {
                var modelo = await _modeloRepository.GetByIdAsync(pedido.IdModelo.Value);
                if (modelo != null)
                    nombreModelo = modelo.Nombre;
            }

            // 8️⃣ Enviar correo (no rompe la creación)
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
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Error al enviar correo: {ex.Message}");
            }

            return Ok(new
            {
                message = "Cita creada correctamente",
                //cita = nuevaCita
                IdCita = nuevaCita.IdCita,
                FechaHora = nuevaCita.FechaCita
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

        [HttpGet("pedido/{idPedido}")]
        public async Task<IActionResult> GetCitaByPedido(int idPedido)
        {
            var cita = await _citaRepository.GetByPedidoIdAsync(idPedido);

            if (cita == null)
                return NotFound();

            return Ok(cita);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CitaDomain citaDomain)
        {

            var citaActual = await _citaRepository.GetByIdAsync(id);
            if (citaActual == null) return NotFound();

            await _citaRepository.UpdateAsync(id, citaDomain);

            if (
                citaActual.Estado == true &&
                citaDomain.Estado == false &&
                citaActual.IdHorario != null
            )
            {
                var horario = await _horarioRepository.GetByIdAsync(citaActual.IdHorario.Value);
                if (horario != null)
                {
                    await _horarioRepository.UpdateAsync(
                        horario.IdHorario,
                        new HorarioDomain
                        {
                            Estado = true // 🔓 liberar
                        }
                    );
                }
            }

            //------------------------------------------------
            if (!citaDomain.PedidoId.HasValue)
                return NoContent(); // o BadRequest si prefieres

            int pedidoId = citaDomain.PedidoId.Value;
            
            var pedido = await _pedidoRepository.GetByIdAsync(pedidoId);
            if (pedido != null)
            {
                var citas = await _citaRepository.GetByPedidoIdAsync(pedidoId);
            
                if (!citas.Any())
                {
                    pedido.IdEstado = 1; // Pendiente
                }
                else if (citas.Any(c => c.Estado == false))
                {
                    pedido.IdEstado = 2; // En proceso
                }
                else
                {
                    pedido.IdEstado = 1; // Pendiente (solo futuras)
                }
            
                await _pedidoRepository.UpdateAsync(pedido.IdPedido, pedido);
            }
            //------------------------------------------------
            return NoContent();
        }

        private DateTime CalcularFechaCita(string dia, TimeSpan horaInicio)
        {
            var today = DateTime.Today;
        
            var map = new Dictionary<string, DayOfWeek>
            {
                { "L", DayOfWeek.Monday },
                { "M", DayOfWeek.Tuesday },
                { "X", DayOfWeek.Wednesday },
                { "J", DayOfWeek.Thursday },
                { "V", DayOfWeek.Friday },
                { "S", DayOfWeek.Saturday },
                { "Lunes", DayOfWeek.Monday },
                { "Martes", DayOfWeek.Tuesday },
                { "Miércoles", DayOfWeek.Wednesday },
                { "Jueves", DayOfWeek.Thursday },
                { "Viernes", DayOfWeek.Friday },
                { "Sábado", DayOfWeek.Saturday },
            };
        
            var targetDay = map[dia];
        
            var daysUntil = ((int)targetDay - (int)today.DayOfWeek + 7) % 7;
            if (daysUntil == 0)
                daysUntil = 7;
        
            var fecha = today.AddDays(daysUntil);
            return fecha.Add(horaInicio);
        }


        [HttpPut("{id}/reprogramar")]
        public async Task<IActionResult> Reprogramar(
            int id,
            [FromBody] ReprogramarCitaRequest request)
        {
            var cita = await _citaRepository.GetByIdAsync(id);
            if (cita == null) return NotFound();
        
            // liberar horario anterior
            if (cita.IdHorario != null)
            {
                var anterior = await _horarioRepository.GetByIdAsync(cita.IdHorario.Value);
                if (anterior != null)
                {
                    anterior.Estado = true;
                    await _horarioRepository.UpdateAsync(
                        anterior.IdHorario,
                        new HorarioDomain
                        {
                            Estado = true
                        }
                    );
                }
            }
        
            // nuevo horario
            var nuevo = await _horarioRepository.GetByIdAsync(request.IdHorarioNuevo);
            if (nuevo == null || nuevo.Estado != true)
                return BadRequest("Horario no disponible");
        
            // ocupar horario nuevo
            nuevo.Estado = false;
            await _horarioRepository.UpdateAsync(
                nuevo.IdHorario,
                new HorarioDomain
                {
                    Estado = false
                }
            );
        
            // 🧠 calcular fecha real
            cita.FechaCita = CalcularFechaCita(
                nuevo.Dia,
                nuevo.HoraInicio.ToTimeSpan()
            );
        
            cita.IdHorario = nuevo.IdHorario;
        
            await _citaRepository.UpdateAsync(
                cita.IdCita,
                new CitaDomain
                {
                    FechaCita = cita.FechaCita,
                    IdHorario = nuevo.IdHorario
                }
            );
        
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
