using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using SilliconValley.Integration.reqres.dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public async Task<List<User>> GetAll()
        {
            string requestUrl = $"{API_URL}";
            List<User> listUsers = new List<User>();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var jsonObject = JObject.Parse(json);                    
                    var usersArray = jsonObject["data"].ToObject<List<User>>();
                    return usersArray;
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
                }
            }
            return listUsers;
        }
        public async Task<User?> GetUserById(int? userId)
        {
            string requestUrl = $"{API_URL}/{userId}"; 
            User? user = null;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                     HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var jsonObject = JObject.Parse(json);
                        user = jsonObject["data"].ToObject<User>();
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogDebug($"Error al llamar a la API: {ex.Message}");
                }
            }
            return user;
        }
    }
}
