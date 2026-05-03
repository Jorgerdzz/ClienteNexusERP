using Microsoft.AspNetCore.Mvc;
using NexusERP.DTOs;
using NexusERP.Extensions;
using Microsoft.AspNetCore.Authorization;
using NexusERP.Models;
using NexusERP.Services;
using NexusERP.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;
using ApiNexusERP.DTOs;

namespace NexusERP.Controllers
{
    [Authorize(Policy = "ADMIN")]
    public class DashboardController : Controller
    {
        private ServiceEstadisticas serviceEstadisticas;

        public DashboardController(ServiceEstadisticas serviceEstadisticas)
        {
            this.serviceEstadisticas = serviceEstadisticas;
        }

        public async Task<IActionResult> Index()
        {
            int anioActual = DateTime.Now.Year;

            MetricasDashboardDTO estadisticas = await this.serviceEstadisticas.GetEstadisticasAsync(anioActual);

            string nombreUsuario = HttpContext.User.FindFirstValue(ClaimTypes.Name);

            HomeDashboardViewModel model = new HomeDashboardViewModel
            {
                NombreUsuario = nombreUsuario,
                TotalFacturadoAnual = estadisticas.TotalFacturadoAnual,
                TotalGastoSalarial = estadisticas.TotalGastoSalarial,
                FacturasPendientes = estadisticas.FacturasPendientes,
                TieneDepartamentos = estadisticas.TieneDepartamentos,
                TieneClientes = estadisticas.TieneClientes,
                TieneEmpleados = estadisticas.TieneEmpleados
            };

            return View(model);
        }
    }
}
