using Microsoft.AspNetCore.Mvc;
using NexusERP.DTOs;
using Microsoft.AspNetCore.Authorization;
using NexusERP.Services;
using NexusERP.ViewModels;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NexusERP.Controllers
{
    [Authorize(Policy = "ADMIN")]
    public class ReportsController : Controller
    {
        private ServiceEstadisticas serviceEstadisticas;

        public ReportsController(ServiceEstadisticas serviceEstadisticas)
        {
            this.serviceEstadisticas = serviceEstadisticas;
        }

        public async Task<IActionResult> Index(int? anio)
        {
            int anioConsulta = anio ?? DateTime.Now.Year;

            ReportsViewModel model = new ReportsViewModel { AnioActual = anioConsulta };

            List<ReporteMensualDto> ingresos = await this.serviceEstadisticas.GetIngresosPorMesAsync(anioConsulta);

            foreach (var item in ingresos)
            {
                model.IngresosMensuales[item.Mes - 1] = item.Total;
            }

            List<ReporteMensualDto> gastos = await this.serviceEstadisticas.GetGastosPorMesAsync(anioConsulta);
            foreach (var item in gastos)
            {
                model.GastosMensuales[item.Mes - 1] = item.Total;
            }

            List<ReporteDepartamentoDto> costesDept = await this.serviceEstadisticas.GetCostesPorDepartamentoAsync(anioConsulta);

            model.DepartamentosNombres = costesDept.Select(c => c.Departamento).ToList();
            model.GastosPorDepartamento = costesDept.Select(c => c.Total).ToList();

            return View(model);
        }
    }
}
