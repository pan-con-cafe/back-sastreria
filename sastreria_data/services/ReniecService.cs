using sastreria_domain.dtos;
using sastreria_domain.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace sastreria_data.services
{
    public class ReniecService : IReniecService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        //private readonly string _apiKey = "apis-token-17106.nDDNdvHbia3JI53XCBS9ZHVWwZvdgIcG"; // Reemplaza por tu API Key real.

        public ReniecService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://api.apis.net.pe/v2/");
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "apis-token-17106.nDDNdvHbia3JI53XCBS9ZHVWwZvdgIcG");
        }

        public async Task<DatosReniecDto?> ObtenerDatosPorDniAsync(string dni)
        {
            //var response = await _httpClient.GetAsync($"reniec/dni?numero={dni}");
            //if (!response.IsSuccessStatusCode) return null;

            //var data = await response.Content.ReadFromJsonAsync<ReniecResponse>();
            //if (data == null) return null;

            //var json = await response.Content.ReadAsStringAsync();
            //var resultadoApi = System.Text.Json.JsonSerializer.Deserialize<DatosReniecDto>(json);

            //return resultadoApi;

            var resultado = await _httpClient.GetFromJsonAsync<DatosReniecDto>(
                $"reniec/dni?numero={dni}", _jsonOptions);

            // Si la API te envía más campos que tu DTO, los ignorará
            return resultado;
        }

        private class ReniecResponse
        {
            public string Nombres { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
        }
    }
}
