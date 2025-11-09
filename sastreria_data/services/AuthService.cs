using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using sastreria_data.database;
using sastreria_domain.RequestResponse;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using sastreria_data.database.tables;

namespace sastreria_data.services
{
    public class AuthService
    {
        private readonly _dbContext _context;
        private readonly IConfiguration _config;

        public AuthService(_dbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public LoginResponse? Login(LoginRequest request)
        {
            var sastre = _context.Sastres.FirstOrDefault(s =>
                s.Correo == request.Correo &&
                s.Contrasenia == request.Contrasenia // Si usas hash, aquí se valida el hash
            );

            if (sastre == null) return null;

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, sastre.Nombre),
            new Claim(ClaimTypes.Email, sastre.Correo),
            new Claim("IdSastre", sastre.IdSastre.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new LoginResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Nombre = sastre.Nombre
            };
        }
    }
}
