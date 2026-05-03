using ApiNexusERP.DTOs;
using NexusERP.DTOs; // Usamos los DTOs, incluyendo el nuevo LibroMayorDTO
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceContabilidad
    {
        private ServiceApi serviceApi;

        public ServiceContabilidad(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<List<CuentaContableDTO>> GetPlanContableAsync()
        {
            return await this.serviceApi.CallGetAsync<List<CuentaContableDTO>>("api/Contabilidad/PlanContable");
        }

        public async Task<CuentaContableDTO> CrearCuentaContableAsync(CuentaContableDTO cuenta)
        {
            return await this.serviceApi.CallPostAsync<CuentaContableDTO>("api/Contabilidad/CrearCuenta", cuenta);
        }

        public async Task<List<AsientoContableDTO>> GetLibroDiarioAsync()
        {
            return await this.serviceApi.CallGetAsync<List<AsientoContableDTO>>("api/Contabilidad/LibroDiario");
        }

        public async Task<LibroMayorDTO> GetLibroMayorAsync(int cuentaId, DateTime desde, DateTime hasta)
        {
            string url = $"api/Contabilidad/LibroMayor/{cuentaId}?desde={desde:yyyy-MM-dd}&hasta={hasta:yyyy-MM-dd}";

            // Usamos nuestro nuevo DTO global para capturar la respuesta
            return await this.serviceApi.CallGetAsync<LibroMayorDTO>(url);
        }
    }
}