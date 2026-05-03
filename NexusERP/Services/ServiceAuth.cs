using ApiNexusERP.DTOs;
using NexusERP.DTOs; // Asegúrate de tener aquí el RegistroDTO
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceAuth
    {
        private ServiceApi serviceApi;

        public ServiceAuth(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<string> LogInUserAsync(string email, string password)
        {
            // Creamos un objeto anónimo al vuelo que coincida con el LoginDTO de tu API
            var loginData = new
            {
                Email = email,
                Password = password
            };

            // Llamamos a la API. Recuerda que la API devuelve un JSON tipo: { "response": "eyJhbGciOi..." }
            var resultado = await this.serviceApi.CallPostAsync<TokenResponseDto>("api/Auth/Login", loginData);

            // Devolvemos solo el string del Token JWT (o null si las credenciales son incorrectas)
            return resultado?.response;
        }

        public async Task<bool> RegisterUserAsync(RegistroDTO model)
        {
            try
            {
                // Enviamos los datos a la API. Toda la lógica transaccional y financiera ocurre en Azure.
                // Si la API devuelve un 200 OK, el objeto no será nulo y el registro habrá sido un éxito.
                // Si devuelve un 400 (ej. email duplicado), el CallPostAsync devolverá null.
                var resultado = await this.serviceApi.CallPostAsync<object>("api/Auth/Register", model);

                return resultado != null;
            }
            catch
            {
                return false;
            }
        }

        // --- CLASE AUXILIAR PRIVADA ---
        // Para cazar la respuesta exacta de tu AuthController en la API que devolvía: new { response = token }
        private class TokenResponseDto
        {
            public string response { get; set; }
        }
    }
}