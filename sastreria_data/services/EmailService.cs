using Azure;
using Microsoft.Extensions.Configuration;
using sastreria_domain.repositories;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sastreria_data.services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"];
            _fromEmail = configuration["SendGrid:FromEmail"];
            _fromName = configuration["SendGrid:FromName"];
        }

        public async Task<bool> EnviarCorreoReservaAsync(
            string destinatario,
            string nombreCliente,
            DateTime fechaCita,
            string horario,
            string modelo)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(destinatario, nombreCliente);
            var subject = "Confirmación de Reserva - Sastrería Estilo";

            var htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
                        .content {{ background-color: #f9f9f9; padding: 20px; border-radius: 5px; margin-top: 20px; }}
                        .info-row {{ margin: 10px 0; padding: 10px; background-color: white; border-left: 4px solid #4CAF50; }}
                        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>¡Reserva Confirmada!</h1>
                        </div>
                        <div class='content'>
                            <p>Hola <strong>{nombreCliente}</strong>,</p>
                            <p>Tu reserva ha sido confirmada exitosamente. Aquí están los detalles:</p>
                            
                            <div class='info-row'>
                                <strong>📅 Fecha:</strong> {fechaCita:dd/MM/yyyy}
                            </div>
                            <div class='info-row'>
                                <strong>🕒 Horario:</strong> {horario}
                            </div>
                            <div class='info-row'>
                                <strong>👔 Modelo:</strong> {modelo}
                            </div>
                            
                            <p style='margin-top: 20px;'>Te esperamos en la fecha indicada. Si necesitas realizar algún cambio, por favor contáctanos.</p>
                        </div>
                        <div class='footer'>
                            <p>Sastrería Estilo - Tu estilo, nuestra pasión</p>
                            <p>Este es un correo automático, por favor no responder.</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            var plainTextContent = $@"
                ¡Reserva Confirmada!
                
                Hola {nombreCliente},
                
                Tu reserva ha sido confirmada exitosamente.
                
                Detalles:
                - Fecha: {fechaCita:dd/MM/yyyy}
                - Horario: {horario}
                - Modelo: {modelo}
                
                Te esperamos en la fecha indicada.
                
                Sastrería Estilo
            ";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            try
            {
                var response = await client.SendEmailAsync(msg);

                var status = response.StatusCode.ToString();
                var body = await response.Body.ReadAsStringAsync();

                Console.WriteLine($"SENDGRID STATUS: {status}");
                Console.WriteLine($"SENDGRID BODY: {body}");

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo: {ex.Message}");
                return false;
            }

        }
    }
}
