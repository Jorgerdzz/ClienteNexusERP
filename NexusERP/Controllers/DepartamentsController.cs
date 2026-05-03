using Microsoft.AspNetCore.Mvc;
using NexusERP.Enums;
using Microsoft.AspNetCore.Authorization;
using NexusERP.Models;
using NexusERP.Models.UI;
using NexusERP.Services;
using NexusERP.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ApiNexusERP.DTOs;

namespace NexusERP.Controllers
{
    [Authorize(Policy = "ADMIN")]
    public class DepartamentsController : Controller
    {
        private ServiceDepartamentos serviceDepartamentos;
        private ServiceEmpleados serviceEmpleados;

        public DepartamentsController(ServiceDepartamentos serviceDepartamentos, ServiceEmpleados serviceEmpleados)
        {
            this.serviceDepartamentos = serviceDepartamentos;
            this.serviceEmpleados = serviceEmpleados;
        }

        public async Task<IActionResult> Index()
        {
            int totalDepartamentos = await this.serviceDepartamentos.GetTotalDepartamentosAsync();
            int totalEmpleados = await this.serviceEmpleados.GetTotalEmpleadosAsync();
            decimal presupuestoAnual = await this.serviceDepartamentos.GetPresupuestoTotalAnualAsync();
            decimal salarioMedioAnual = await this.serviceEmpleados.GetSalarioPromedioAnualAsync();

            var estadisticas = await this.serviceDepartamentos.GetEstadisticasDepartamentosAsync();

            IndexDepartamentosViewModel model = new IndexDepartamentosViewModel
            {
                TotalDepartamentos = totalDepartamentos,
                TotalEmpleadosGlobal = totalEmpleados,
                PresupuestoTotalGlobalAnual = presupuestoAnual,
                PresupuestoTotalGlobalMensual = presupuestoAnual / 12,
                SalarioPromedioGlobalAnual = salarioMedioAnual,
                SalarioPromedioGlobalMensual = salarioMedioAnual / 12,

                Departamentos = estadisticas.Select(e => new DepartamentoCardViewModel
                {
                    Id = e.Id,
                    Nombre = e.Nombre,
                    PresupuestoAnual = e.PresupuestoAnual,
                    PresupuestoMensual = e.PresupuestoAnual / 12,
                    NumeroEmpleados = e.NumeroEmpleados, 
                    SalarioPromedio = e.SalarioPromedio
                }).ToList()
            };
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            DepartamentoDTO dep = await this.serviceDepartamentos.FindDepartamentoAsync(id);
            if (dep == null) return NotFound();
            List<EmpleadoDTO> empleadosDTO = await this.serviceEmpleados.GetEmpleadosDepartamentoAsync(id);
            List<Empleado> empleados = empleadosDTO.Select(e => new Empleado { Id = e.Id, Nombre = e.Nombre, Apellidos = e.Apellidos, SalarioBrutoAnual = e.SalarioBrutoAnual }).ToList();
            DepartamentoDetailsViewModel model = new DepartamentoDetailsViewModel
            {
                Id = dep.Id,
                Nombre = dep.Nombre,
                PresupuestoAnual = dep.PresupuestoAnual,
                PresupuestoMensual = dep.PresupuestoAnual / 12,
                NumeroEmpleados = empleados.Count(),
                Empleados = empleados,
                SalarioPromedioAnual = empleados.Any() ? empleados.Average(e => e.SalarioBrutoAnual) : 0,
                SalarioPromedioMensual = empleados.Any() ? empleados.Average(e => e.SalarioBrutoAnual) / 12 : 0
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDepartamentoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                AlertService.Warning(TempData, "Por favor, revisa los campos del formulario.");
                return RedirectToAction("Index");
            }

            DepartamentoDTO nuevoDepartamento = new DepartamentoDTO
            {
                Nombre = model.Nombre,
                PresupuestoAnual = model.PresupuestoAnual
            };
            var creado = await this.serviceDepartamentos.CreateDepartamentoAsync(nuevoDepartamento);
            if (creado != null)
            {
                AlertService.Success(TempData, $"El departamento '{model.Nombre}' se ha creado correctamente.");
            }
            else
            {
                AlertService.Error(TempData, "Error al crear el departamento");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditDepartamentoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                AlertService.Warning(TempData, "Datos inválidos. No se pudo actualizar el departamento.");
                return RedirectToAction("Index");
            }

            DepartamentoDTO departamentoActualizado = new DepartamentoDTO
            {
                Id = model.Id,
                Nombre = model.Nombre,
                PresupuestoAnual = model.PresupuestoAnual
            };

            bool actualizado = await this.serviceDepartamentos.UpdateDepartamentoAsync(departamentoActualizado);

            if (actualizado)
            {
                AlertService.Toast(TempData, "Departamento actualizado correctamente", "success");
            }
            else
            {
                AlertService.Error(TempData, "No se pudo actualizar. El departamento no existe.");
            }

            return RedirectToAction("Index");
        
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool eliminado = await this.serviceDepartamentos.DeleteDepartamentoAsync(id);
            if (eliminado)
            {
                AlertService.Success(TempData, "Departamento eliminado correctamente");
                return Ok();
            }
            else
            {
                return BadRequest("No se puede eliminar el departamento. Asegúrate de que no tenga empleados asignados antes de borrarlo.");
            }
        }

    }
}
