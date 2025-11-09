using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_domain.RequestResponse
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string Nombre { get; set; } = null!;
    }
}
