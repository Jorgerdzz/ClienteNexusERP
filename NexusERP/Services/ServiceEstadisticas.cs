using ApiNexusERP.DTOs;
using NexusERP.DTOs; // Asegúrate de tener copiados los 3 DTOs de reportes en tu MVC
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceEstadisticas
    {
        private ServiceApi serviceApi;

        public ServiceEstadisticas(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<List<ReporteMensualDto>> GetIngresosPorMesAsync(int anio)
        {
            // Gracias al [action]/{anio}, la ruta es directa y muy limpia
            return await this.serviceApi.CallGetAsync<List<ReporteMensualDto>>($"api/Estadisticas/Ingresos/{anio}");
        }

        public async Task<List<ReporteMensualDto>> GetGastosPorMesAsync(int anio)
        {
            return await this.serviceApi.CallGetAsync<List<ReporteMensualDto>>($"api/Estadisticas/Gastos/{anio}");
        }

        public async Task<List<ReporteDepartamentoDto>> GetCostesPorDepartamentoAsync(int anio)
        {
            return await this.serviceApi.CallGetAsync<List<ReporteDepartamentoDto>>($"api/Estadisticas/CostesDepartamento/{anio}");
        }

        public async Task<MetricasDashboardDTO> GetEstadisticasAsync(int anio)
        {
            return await this.serviceApi.CallGetAsync<MetricasDashboardDTO>($"api/Estadisticas/MetricasGlobales/{anio}");
        }
    }
}