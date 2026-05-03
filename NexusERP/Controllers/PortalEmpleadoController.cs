using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusERP.Enums;
using NexusERP.Extensions;
using NexusERP.Models;
using NexusERP.Services;
using NexusERP.ViewModels;
using System.Security.Claims;
using ApiNexusERP.DTOs;
using System.Linq;

namespace NexusERP.Controllers
{
    public class PortalEmpleadoController : Controller
    {
        private ServiceNominas serviceNominas;

        public PortalEmpleadoController(ServiceNominas serviceNominas)
        {
            this.serviceNominas = serviceNominas;
        }

        [Authorize(Policy = "EMPLEADO")]
        public IActionResult Index()
        {
            string nombreUsuario = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            ViewBag.NombreEmpleado = nombreUsuario;
            return View();
        }

        [Authorize(Policy = "DESCARGARPDF")]
        public async Task<IActionResult> MisNominas(int? mes, int? anio, int? empleadoId)
        {
            int idEmpleadoFinal;

            if (User.IsInRole(RolesUsuario.Admin.ToString()) && empleadoId.HasValue)
            {
                idEmpleadoFinal = empleadoId.Value;
                ViewBag.EmpleadoId = empleadoId.Value;
            }
            else
            {
                string idEmpleadoString = HttpContext.User.FindFirstValue("EmpleadoId");
                idEmpleadoFinal = int.Parse(idEmpleadoString);
            }

            int mesConsulta = mes ?? DateTime.Now.Month;
            int anioConsulta = anio ?? DateTime.Now.Year;

            NominaDTO f = await this.serviceNominas.FindNominaMesAsync(idEmpleadoFinal, mesConsulta, anioConsulta);

            Nomina nomina = null;
            if (f != null)
            {
                nomina = new Nomina
                {
                    Id = f.Id,
                    EmpleadoId = f.EmpleadoId,
                    Empleado = new Empleado { Nombre = f.NombreCompletoEmpleado, Dni = f.DniEmpleado, Apellidos = "" },
                    Mes = f.Mes,
                    Anio = f.Anio,
                    FechaInicio = f.FechaInicio,
                    FechaFin = f.FechaFin,
                    BaseCotizacionCc = f.BaseCotizacionCc,
                    BaseCotizacionCp = f.BaseCotizacionCp,
                    BaseIrpf = f.BaseIrpf,
                    PorcentajeIrpf = f.PorcentajeIrpf,
                    TotalDevengado = f.TotalDevengado,
                    TotalDeducciones = f.TotalDeducciones,
                    LiquidoApercibir = f.LiquidoApercibir,
                    SsEmpresaContingenciasComunes = f.SsEmpresaContingenciasComunes,
                    SsEmpresaAccidentesTrabajo = f.SsEmpresaAccidentesTrabajo,
                    SsEmpresaDesempleo = f.SsEmpresaDesempleo,
                    SsEmpresaFormacion = f.SsEmpresaFormacion,
                    SsEmpresaFogasa = f.SsEmpresaFogasa,
                    SsEmpresaMei = f.SsEmpresaMei,
                    SsEmpresaHorasExtras = f.SsEmpresaHorasExtras,
                    SsEmpresaTotal = f.SsEmpresaTotal,
                    Estado = f.Estado,
                    NominaDetalles = f.Detalles?.Select(d => new NominaDetalle
                    {
                        Id = d.Id,
                        Codigo = d.Codigo,
                        ConceptoNombre = d.ConceptoNombre,
                        Importe = d.Importe,
                        Tipo = d.Tipo
                    }).ToList()
                };
            }

            MisNominasViewModel model = new MisNominasViewModel
            {
                MesSeleccionado = mesConsulta,
                AnoSeleccionado = anioConsulta,
                NominaActual = nomina
            };
            return View(model);
        }
    }
}
