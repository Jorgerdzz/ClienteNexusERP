using Microsoft.AspNetCore.Mvc;
using NexusERP.Extensions;
using Microsoft.AspNetCore.Authorization;
using NexusERP.Models;
using NexusERP.Services;
using NexusERP.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;
using ApiNexusERP.DTOs;

namespace NexusERP.Controllers
{
    [Authorize(Policy = "ADMIN")]
    public class SettingsController : Controller
    {
        private ServiceEmpresas serviceEmpresas;

        public SettingsController(ServiceEmpresas serviceEmpresas)
        {
            this.serviceEmpresas = serviceEmpresas;
        }

        public async Task<IActionResult> Index()
        {
            EmpresaDTO empresa = await this.serviceEmpresas.FindEmpresaAsync();
            if (empresa == null) return NotFound();
            SettingsViewModel model = new SettingsViewModel
            {
                EmpresaId = empresa.Id,
                NombreComercial = empresa.NombreComercial,
                RazonSocial = empresa.RazonSocial,
                CIF = empresa.Cif,
                FechaAlta = empresa.FechaAlta?.ToString("dd MMM yyyy"),
                Activo = empresa.Activo
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateEmpresa(SettingsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                AlertService.Warning(TempData, "Por favor, revisa los datos introducidos.");
                return RedirectToAction("Index");
            }

            EmpresaDTO empresaActualizada = new EmpresaDTO
            {
                Id = model.EmpresaId,
                NombreComercial = model.NombreComercial,
                RazonSocial = model.RazonSocial,
                Cif = model.CIF
            };

            bool actualizado = await this.serviceEmpresas.UpdateEmpresaAsync(empresaActualizada);

            if (actualizado)
            {
                AlertService.Toast(TempData, "Datos fiscales de la empresa actualizados", "success");
            }
            else
            {
                AlertService.Error(TempData, "No se pudo actualizar. Es posible que el CIF ya esté registrado.");
            }

            return RedirectToAction("Index");

        }

    }
}
