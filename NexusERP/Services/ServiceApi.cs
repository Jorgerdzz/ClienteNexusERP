using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace NexusERP.Services
{
    public class ServiceApi
    {
        private HttpClient httpClient;
        private IHttpContextAccessor contextAccessor;

        // Inyectamos el HttpClient (que configuraremos en Program.cs) 
        // y el Accesor para poder leer el Token de la sesión actual
        public ServiceApi(HttpClient httpClient, IHttpContextAccessor contextAccessor)
        {
            this.httpClient = httpClient;
            this.contextAccessor = contextAccessor;
        }

        // Método privado para añadir el Token a la cabecera antes de cada petición
        private void AddTokenHeader()
        {
            this.httpClient.DefaultRequestHeaders.Clear();
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Si es la ruta de Login o Register, NO intentamos leer el Token
            // Porque la sesión todavía no está iniciada (se rompería NullReferenceException)
            string token = this.contextAccessor.HttpContext?.Session?.GetString("TOKEN");

            if (!string.IsNullOrEmpty(token))
            {
                this.httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            }
        }

        // --- MÉTODOS GENÉRICOS HTTP ---

        // Petición GET
        public async Task<T> CallGetAsync<T>(string requestUri)
        {
            this.AddTokenHeader();
            HttpResponseMessage response = await this.httpClient.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                return default(T);
            }
        }

        // Petición POST (Recibe un objeto, lo envía como JSON y devuelve la respuesta)
        public async Task<T> CallPostAsync<T>(string requestUri, object model)
        {
            this.AddTokenHeader();
            HttpResponseMessage response = await this.httpClient.PostAsJsonAsync(requestUri, model);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>();
            }
            else
            {
                return default(T);
            }
        }

        // Petición PUT (Actualizar)
        public async Task CallPutAsync(string requestUri, object model)
        {
            this.AddTokenHeader();
            await this.httpClient.PutAsJsonAsync(requestUri, model);
        }

        // Petición DELETE (Borrar)
        public async Task CallDeleteAsync(string requestUri)
        {
            this.AddTokenHeader();
            await this.httpClient.DeleteAsync(requestUri);
        }
    }
}