using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.RequestResponse
{
    public class LoginRequest
    {
        public string Correo { get; set; } = null!;
        public string Contrasenia { get; set; } = null!;
    }
}
