using ApiNexusERP.DTOs;
using NexusERP.DTOs; // Usamos el DTO de tu API
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceEmpresas
    {
        private ServiceApi serviceApi;

        public ServiceEmpresas(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<EmpresaDTO> FindEmpresaAsync()
        {
            // Llamamos a la raíz del endpoint. La API ya sabe qué empresa devolver leyendo el Token.
            return await this.serviceApi.CallGetAsync<EmpresaDTO>("api/Empresas");
        }

        public async Task<bool> UpdateEmpresaAsync(EmpresaDTO empresa)
        {
            try
            {
                // Enviamos el DTO completo. No hay ID en la URL porque tu [HttpPut] de la API no lo requiere.
                await this.serviceApi.CallPutAsync("api/Empresas", empresa);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmpresaAsync()
        {
            try
            {
                // De nuevo, no mandamos ID. La API lee el token, sabe qué empresa es, y ejecuta el borrado.
                await this.serviceApi.CallDeleteAsync("api/Empresas");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}