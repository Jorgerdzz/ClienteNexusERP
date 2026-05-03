using NexusERP.DTOs; // Asegúrate de tener copiado el SearchResultDTO en tu proyecto MVC
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceBusqueda
    {
        private ServiceApi serviceApi;

        public ServiceBusqueda(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<List<SearchResultDto>> BuscarGlobalAsync(string query)
        {
            // Si el usuario borra el texto del buscador y lo deja vacío, 
            // no molestamos a la API, devolvemos una lista vacía directamente.
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<SearchResultDto>();
            }

            // Llamamos al controlador de búsqueda de tu API pasando la query por la URL
            // Asumiendo que tu endpoint es: GET /api/Search?q={texto}
            var resultados = await this.serviceApi.CallGetAsync<List<SearchResultDto>>($"api/Busqueda?q={query}");

            // Si la API no responde o da error, devolvemos una lista vacía para que no crashee el desplegable
            return resultados ?? new List<SearchResultDto>();
        }
    }
}