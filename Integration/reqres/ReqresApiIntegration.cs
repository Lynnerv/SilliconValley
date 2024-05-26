using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using SilliconValley.Integration.reqres.dto;
using Newtonsoft.Json;

namespace SilliconValley.Integration.reqres
{
    public class ReqresApiIntegration
    {       
        private readonly ILogger<ReqresApiIntegration> _logger;
        private const string API_URL="https://reqres.in/api/users";
        public ReqresApiIntegration(ILogger<ReqresApiIntegration> logger)
        {
            _logger = logger;
        }
         public async Task<String> CreateUser(User newUser)
        {
            String? msj="";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                        var jsonContent = JsonConvert.SerializeObject(newUser);
                        var content = new StringContent(jsonContent,Encoding.UTF8,"application/json");
                        HttpResponseMessage response = await client.PostAsync(API_URL, content);
                if (response.IsSuccessStatusCode)
                    {
                        _logger.LogInformation($"Usuario creado:{response}");
                        msj="Usuario creado"+response;
                    }
                else
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        _logger.LogError($"Error al crear el usuario: {responseContent}");
                        msj=$"ERROR:{responseContent}";
                    }
                }
                catch (HttpRequestException ex)
                {
                        _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
                }
            }
            return msj;
        }
    }
}
