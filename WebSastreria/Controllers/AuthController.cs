using Microsoft.AspNetCore.Mvc;
using sastreria_data.services;
using sastreria_domain.RequestResponse;

namespace WebSastreria.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var result = _authService.Login(request);
            if (result == null)
                return Unauthorized(new { message = "Correo o contraseña incorrectos" });

            return Ok(result);
        }
    }
}
