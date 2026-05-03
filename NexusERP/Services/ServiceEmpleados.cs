using ApiNexusERP.DTOs;
using NexusERP.DTOs; // Asegúrate de tener copiado tu EmpleadoDTO en el MVC
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceEmpleados
    {
        private ServiceApi serviceApi;

        public ServiceEmpleados(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<List<EmpleadoDTO>> GetEmpleadosAsync()
        {
            return await this.serviceApi.CallGetAsync<List<EmpleadoDTO>>("api/Empleados");
        }

        public async Task<List<EmpleadoDTO>> GetEmpleadosDepartamentoAsync(int idDepartamento)
        {
            // Gracias al [action]/{id} de tu API, la ruta queda así:
            return await this.serviceApi.CallGetAsync<List<EmpleadoDTO>>($"api/Empleados/EmpleadosDepartamento/{idDepartamento}");
        }

        public async Task<EmpleadoDTO> FindEmpleadoAsync(int idEmpleado)
        {
            return await this.serviceApi.CallGetAsync<EmpleadoDTO>($"api/Empleados/FindEmpleado/{idEmpleado}");
        }

        public async Task<int> GetTotalEmpleadosAsync()
        {
            return await this.serviceApi.CallGetAsync<int>("api/Empleados/NumeroTotalEmpleados");
        }

        public async Task<decimal> GetSalarioPromedioAnualAsync()
        {
            return await this.serviceApi.CallGetAsync<decimal>("api/Empleados/SalarioPromedioAnual");
        }

        public async Task<decimal> GetSalarioPromedioAnualPorDepartamentoAsync(int idDepartamento)
        {
            return await this.serviceApi.CallGetAsync<decimal>($"api/Empleados/SalarioPromedioAnualByDepartamento/{idDepartamento}");
        }

        public async Task<EmpleadoDTO> CreateEmpleadoAsync(EmpleadoDTO empleado)
        {
            // Enviamos el DTO al POST. La API hará toda la magia de crear el Usuario, la Seguridad y la Transacción.
            return await this.serviceApi.CallPostAsync<EmpleadoDTO>("api/Empleados", empleado);
        }

        public async Task<bool> UpdateEmpleadoAsync(EmpleadoDTO empleado)
        {
            try
            {
                // En tu API, el PUT no recibe ID en la URL, recibe todo en el Body (DTO)
                await this.serviceApi.CallPutAsync("api/Empleados", empleado);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmpleadoAsync(int id)
        {
            try
            {
                // La API borrará al empleado, sus conceptos fijos y su usuario de acceso en cascada
                await this.serviceApi.CallDeleteAsync($"api/Empleados/{id}");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}