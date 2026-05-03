using NugetModelsNexusERP.Models; // Usamos el modelo de tu NuGet
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceClientes
    {
        private ServiceApi serviceApi;

        public ServiceClientes(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            // La API ya se encargará internamente de filtrar por el EmpresaId del token
            // y de hacer el .OrderBy(c => c.RazonSocial) antes de devolvernos el JSON.
            return await this.serviceApi.CallGetAsync<List<Cliente>>("api/Clientes");
        }

        public async Task<Cliente> FindClienteAsync(int idCliente)
        {
            return await this.serviceApi.CallGetAsync<Cliente>($"api/Clientes/{idCliente}");
        }

        public async Task<Cliente> CreateClienteAsync(Cliente cliente)
        {
            return await this.serviceApi.CallPostAsync<Cliente>("api/Clientes", cliente);
        }

        public async Task<Cliente> UpdateClienteAsync(Cliente cliente)
        {
            // Hacemos la petición PUT. Asumimos que tu controlador en la API recibe el ID por URL
            await this.serviceApi.CallPutAsync($"api/Clientes/{cliente.Id}", cliente);

            // Devolvemos el mismo cliente para mantener la firma exacta que tenía tu MVC
            return cliente;
        }

        public async Task<bool> DeleteClienteAsync(int idCliente)
        {
            try
            {
                await this.serviceApi.CallDeleteAsync($"api/Clientes/{idCliente}");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}