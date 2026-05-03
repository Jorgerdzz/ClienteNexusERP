using ApiNexusERP.DTOs;
using NexusERP.DTOs; // Tus DTOs de Nómina, Detalles y Empleados
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceNominas
    {
        private ServiceApi serviceApi;

        public ServiceNominas(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<List<EmpleadoDTO>> GetEstadoNominasAsync(int mes, int anio)
        {
            // Apunta al endpoint [action]/{mes}/{anio}
            return await this.serviceApi.CallGetAsync<List<EmpleadoDTO>>($"api/Nominas/Estado/{mes}/{anio}");
        }

        public async Task<NominaDTO> FindNominaAsync(int idNomina)
        {
            // Endpoint estándar con ID
            return await this.serviceApi.CallGetAsync<NominaDTO>($"api/Nominas/{idNomina}");
        }

        public async Task<NominaDTO> FindNominaMesAsync(int idEmpleado, int mes, int anio)
        {
            // Apunta al endpoint con [action]
            return await this.serviceApi.CallGetAsync<NominaDTO>($"api/Nominas/FindNominaMes/{idEmpleado}/{mes}/{anio}");
        }

        public async Task<NominaDTO> GenerarNominaAsync(NominaDTO nomina)
        {
            // Le mandamos el DTO con los cálculos base y la API genera toda la contabilidad, 
            // el control de gastos y la transacción.
            return await this.serviceApi.CallPostAsync<NominaDTO>("api/Nominas/Generar", nomina);
        }

        public async Task<bool> PagarNominaAsync(int idNomina)
        {
            try
            {
                // Igual que con las Facturas: el PUT de tu API solo necesita el ID por URL.
                // Le pasamos un objeto anónimo vacío (new {}) para cumplir con la firma del HttpClient
                await this.serviceApi.CallPutAsync($"api/Nominas/Pagar/{idNomina}", new { });
                return true;
            }
            catch
            {
                // Si la API falla (ej. faltan las cuentas 572 o 465, o ya está pagada), devolvemos false
                return false;
            }
        }
    }
}