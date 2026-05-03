using ApiNexusERP.DTOs;
using NexusERP.DTOs; // Usamos los DTOs oficiales
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceDepartamentos
    {
        private ServiceApi serviceApi;

        public ServiceDepartamentos(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<List<DepartamentoDTO>> GetDepartamentosAsync()
        {
            return await this.serviceApi.CallGetAsync<List<DepartamentoDTO>>("api/Departamentos");
        }

        public async Task<DepartamentoDTO> FindDepartamentoAsync(int idDepartamento)
        {
            return await this.serviceApi.CallGetAsync<DepartamentoDTO>($"api/Departamentos/{idDepartamento}");
        }

        public async Task<DepartamentoDTO> CreateDepartamentoAsync(DepartamentoDTO departamento)
        {
            return await this.serviceApi.CallPostAsync<DepartamentoDTO>("api/Departamentos", departamento);
        }

        public async Task<bool> UpdateDepartamentoAsync(DepartamentoDTO departamento)
        {
            try
            {
                // El [HttpPut] de tu API no lleva {id} en la ruta, recibe todo por el body
                await this.serviceApi.CallPutAsync("api/Departamentos", departamento);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteDepartamentoAsync(int idDepartamento)
        {
            try
            {
                await this.serviceApi.CallDeleteAsync($"api/Departamentos/{idDepartamento}");
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> GetTotalDepartamentosAsync()
        {
            return await this.serviceApi.CallGetAsync<int>("api/Departamentos/numerototal");
        }

        public async Task<decimal> GetPresupuestoTotalAnualAsync()
        {
            return await this.serviceApi.CallGetAsync<decimal>("api/Departamentos/presupuestototal");
        }

        public async Task<List<EstadisticasDepartamentoDTO>> GetEstadisticasDepartamentosAsync()
        {
            // Usamos tu nuevo DTO global. 
            // Ya no devolvemos una Tupla, devolvemos una lista de DTOs limpia y profesional.
            return await this.serviceApi.CallGetAsync<List<EstadisticasDepartamentoDTO>>("api/Departamentos/estadisticas");
        }
    }
}