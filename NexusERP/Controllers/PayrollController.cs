using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusERP.Enums;
using NexusERP.Helpers;
using NexusERP.Models;
using NexusERP.Services;
using NexusERP.ViewModels;
using QuestPDF.Fluent;
using System.Threading.Tasks;
using ApiNexusERP.DTOs;
using System.Linq;
using System.Collections.Generic;
using System;

namespace NexusERP.Controllers
{
    public class PayrollController : Controller
    {
        private ServiceNominas serviceNominas;
        private ServiceEmpleados serviceEmpleados;
        private HelperSessionContextAccessor contextAccessor;

        public PayrollController(ServiceNominas serviceNominas, ServiceEmpleados serviceEmpleados, HelperSessionContextAccessor contextAccessor)
        {
            this.serviceNominas = serviceNominas;
            this.serviceEmpleados = serviceEmpleados;
            this.contextAccessor = contextAccessor;
        }

        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> Index(int? mes, int? anio)
        {
            int mesFiltro = mes ?? DateTime.Now.Month;
            int anioFiltro = anio ?? DateTime.Now.Year;

            List<EmpleadoDTO> empleados = await this.serviceNominas.GetEstadoNominasAsync(mesFiltro, anioFiltro);

            NominasIndexViewModel model = new NominasIndexViewModel
            {
                MesSeleccionado = mesFiltro,
                AnoSeleccionado = anioFiltro,
                Empleados = new List<EmpleadoNominaRowViewModel>()
            };

            if(empleados != null)
            {
                foreach(var emp in empleados)
                {
                    NominaDTO nominaDelMes = await this.serviceNominas.FindNominaMesAsync(emp.Id, mesFiltro, anioFiltro);

                    EmpleadoNominaRowViewModel empNomina = new EmpleadoNominaRowViewModel
                    {
                        EmpleadoId = emp.Id,
                        NombreCompleto = $"{emp.Nombre} {emp.Apellidos}".Trim(),
                        Email = emp.EmailCorporativo,
                        DepartamentoNombre = emp.NombreDepartamento ?? "Sin Asignar",
                        SalarioBrutoAnual = emp.SalarioBrutoAnual,
                        EstaCalculada = nominaDelMes != null,
                        NominaId = nominaDelMes?.Id,
                        LiquidoAPercibir = nominaDelMes?.LiquidoApercibir,
                        Estado = nominaDelMes?.Estado ?? "Pendiente"
                    };
                    model.Empleados.Add(empNomina);
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> Calcular(int empleadoId, int mes, int anio)
        {
            EmpleadoDTO empleado = await this.serviceEmpleados.FindEmpleadoAsync(empleadoId);

            if (empleado == null)
            {
                return NotFound("El empleado no existe o no tienes permisos.");
            }

            // Usamos un estado civil por defecto porque no viene en el DTO simplificado
            decimal porcentajeIrpf = CalcularPorcentajeIRPF(
                empleado.SalarioBrutoAnual,
                0, // numeroHijos
                0, // porcentajeDiscapacidad
                EstadoCivil.Soltero // estadoCivil
            );

            // 3. Crear el paquete (ViewModel) para la vista
            CalcularNominaViewModel model = new CalcularNominaViewModel
            {
                EmpleadoId = empleado.Id,
                Mes = mes,
                Anio = anio,
                EmpleadoNombre = $"{empleado.Nombre} {empleado.Apellidos}".Trim(),
                DepartamentoNombre = empleado.NombreDepartamento ?? "Sin Asignar",
                SalarioBrutoAnual = empleado.SalarioBrutoAnual,
                SalarioMensualSugerido = empleado.SalarioBrutoAnual / 14,
                FechaInicio = new DateOnly(anio, mes, 1),
                FechaFin = new DateOnly(anio, mes, DateTime.DaysInMonth(anio, mes)),
                PorcentajeIRPF = porcentajeIrpf,
                Conceptos = new List<ConceptoNominaItemViewModel>()
            };

            model.EmpleadoIniciales = model.EmpleadoNombre.ObtenerIniciales();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> GuardarNomina(CalcularNominaViewModel model)
        {
            int idEmpresa = this.contextAccessor.GetEmpresaIdSession();
            NominaDTO nomina = new NominaDTO
            {
                EmpleadoId = model.EmpleadoId,
                Mes = model.Mes,
                Anio = model.Anio,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,

                BaseCotizacionCc = model.BaseCotizacion_CC,
                BaseCotizacionCp = model.BaseCotizacion_CP,
                BaseIrpf = model.BaseIRPF,
                PorcentajeIrpf = model.PorcentajeIRPF,

                TotalDevengado = model.TotalDevengado,
                TotalDeducciones = model.TotalDeducciones,
                LiquidoApercibir = model.LiquidoAPercibir,

                SsEmpresaContingenciasComunes = model.SS_Empresa_ContingenciasComunes,
                SsEmpresaAccidentesTrabajo = model.SS_Empresa_AccidentesTrabajo,
                SsEmpresaDesempleo = model.SS_Empresa_Desempleo,
                SsEmpresaFormacion = model.SS_Empresa_Formacion,
                SsEmpresaFogasa = model.SS_Empresa_Fogasa,
                SsEmpresaMei = model.SS_Empresa_MEI,
                SsEmpresaHorasExtras = model.SS_Empresa_HorasExtras,

                SsEmpresaTotal = model.SS_Empresa_Total,

                Estado = "Pendiente",

                FechaGeneracion = DateTime.Now
            };

            nomina.Detalles = new List<NominaDetalleDTO>();

            void AgregarDetalle(string codigo, string nombre, decimal importe, int tipo)
            {
                if (importe > 0)
                {
                    nomina.Detalles.Add(new NominaDetalleDTO
                    {
                        Codigo = codigo,
                        ConceptoNombre = nombre,
                        Importe = importe,
                        Tipo = tipo 
                    });
                }
            }

            AgregarDetalle("SAL_BASE", "Salario Base", model.SalarioBase, 1);
            AgregarDetalle("INCENTIVOS", "Incentivos", model.Incentivos, 1);
            AgregarDetalle("PLUS_DED", "Plus especial dedicación", model.PlusDedicacion, 1);
            AgregarDetalle("PLUS_ANT", "Plus antigüedad", model.PlusAntiguedad, 1);
            AgregarDetalle("PLUS_ACT", "Plus actividad", model.PlusActividad, 1);
            AgregarDetalle("PLUS_NOC", "Plus nocturnidad", model.PlusNocturnidad, 1);
            AgregarDetalle("PLUS_RES", "Plus responsabilidad", model.PlusResponsabilidad, 1);
            AgregarDetalle("PLUS_CONV", "Plus convenio", model.PlusConvenio, 1);
            AgregarDetalle("PLUS_IDIOM", "Plus idiomas", model.PlusIdiomas, 1);
            AgregarDetalle("H_EXTRA", "Horas extraordinarias", model.HorasExtraordinarias, 1);
            AgregarDetalle("H_COMP", "Horas complementarias", model.HorasComplementarias, 1);
            AgregarDetalle("SAL_ESP", "Salario en especie", model.SalarioEspecie, 1);

            // Devengos No Salariales (Tipo 1)
            AgregarDetalle("IND_SUP", "Indemnizaciones o Suplidos", model.IndemnizacionesSuplidos, 1);
            AgregarDetalle("PREST_SS", "Prestaciones S.S.", model.PrestacionesSS, 1);
            AgregarDetalle("IND_DESP", "Indemnizaciones por despido", model.IndemnizacionesDespido, 1);
            AgregarDetalle("PLUS_TRANS", "Plus transporte", model.PlusTransporte, 1);
            AgregarDetalle("DIETAS", "Dietas", model.Dietas, 1);

            // Deducciones Trabajador (Tipo 2)
            AgregarDetalle("DED_CC", "Contingencias Comunes", model.SS_ContingenciasComunes, 2);
            AgregarDetalle("DED_MEI", "M.E.I.", model.SS_MEI, 2);
            AgregarDetalle("DED_DES", "Desempleo", model.SS_Desempleo, 2);
            AgregarDetalle("DED_FP", "Formación Profesional", model.SS_Formacion, 2);
            AgregarDetalle("DED_IRPF", "Retención I.R.P.F.", model.RetencionIRPF, 2);

            var res = await this.serviceNominas.GenerarNominaAsync(nomina);

            if (res != null)
            {
                AlertService.Toast(TempData, "Nómina generada correctamente.");
                return RedirectToAction("Details", new { idNomina = res.Id});
            }
            else
            {
                AlertService.Error(TempData, "Hubo un error al generar la nómina.");
                return RedirectToAction("Calcular", new { empleadoId = model.EmpleadoId, mes = model.Mes, anio = model.Anio });
            }
        }

        [HttpPost]
        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> RegistrarPago(int idNomina)
        {
            bool resultado = await this.serviceNominas.PagarNominaAsync(idNomina);

            if (resultado)
            {
                AlertService.Toast(TempData, "Nómina abonada correctamente.", "success");
            }
            else
            {
                AlertService.Error(TempData, "Hubo un error al abonar la nómina.");
            }

            return RedirectToAction("Index");
        }

        [Authorize(Policy = "ADMIN")]
        public async Task<IActionResult> Details(int idNomina)
        {
            NominaDTO f = await this.serviceNominas.FindNominaAsync(idNomina);
            if(f == null) return NotFound();
            
            Nomina nomina = new Nomina
            {
                Id = f.Id,
                EmpleadoId = f.EmpleadoId,
                Empleado = new Empleado { Nombre = f.NombreCompletoEmpleado, Dni = f.DniEmpleado },
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
                NominaDetalles = f.Detalles.Select(d => new NominaDetalle
                {
                    Id = d.Id,
                    Codigo = d.Codigo,
                    ConceptoNombre = d.ConceptoNombre,
                    Importe = d.Importe,
                    Tipo = d.Tipo
                }).ToList()
            };
            
            return View(nomina);
        }

        [Authorize(Policy = "DESCARGARPDF")]
        public async Task<IActionResult> DescargarPdf(int idNomina)
        {
            NominaDTO f = await this.serviceNominas.FindNominaAsync(idNomina);

            if (f == null) return NotFound();

            Nomina nomina = new Nomina
            {
                Id = f.Id,
                EmpleadoId = f.EmpleadoId,
                Empleado = new Empleado { Nombre = f.NombreCompletoEmpleado, Apellidos = "", Dni = f.DniEmpleado },
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
                NominaDetalles = f.Detalles.Select(d => new NominaDetalle
                {
                    Id = d.Id,
                    Codigo = d.Codigo,
                    ConceptoNombre = d.ConceptoNombre,
                    Importe = d.Importe,
                    Tipo = d.Tipo
                }).ToList()
            };

            // Usamos la magia de QuestPDF
            var document = new NominaDocument(nomina);
            byte[] pdfBytes = document.GeneratePdf();

            string nombreArchivo = $"Nomina_{nomina.Empleado.Nombre}_{nomina.Empleado.Apellidos}_{nomina.Anio}_{nomina.Mes:D2}.pdf";

            return File(pdfBytes, "application/pdf", nombreArchivo);
        }



        private decimal CalcularPorcentajeIRPF(decimal salarioBrutoAnual, int numeroHijos, int porcentajeDiscapacidad, EstadoCivil estadoCivil)
        {
            // 1. Determinar situación familiar
            string situacionFamiliar = "S1"; // Soltero sin hijos por defecto
            bool esCasado = (estadoCivil == EstadoCivil.Casado);

            if (esCasado)
            {
                if (numeroHijos >= 2) situacionFamiliar = "M3";
                else if (numeroHijos == 1) situacionFamiliar = "M2";
                else situacionFamiliar = "M1";
            }
            else
            {
                // Esto aplica a Solteros, Divorciados y Viudos
                if (numeroHijos > 0) situacionFamiliar = "S2";
            }

            // 2. Aplicar reducciones por discapacidad
            decimal reduccionDiscapacidad = 0;
            if (porcentajeDiscapacidad >= 65)
            {
                reduccionDiscapacidad = 4000;
            }
            else if (porcentajeDiscapacidad >= 33)
            {
                reduccionDiscapacidad = 2000;
            }

            // 3. Base de cálculo (salario anual - reducciones)
            decimal baseCalculo = Math.Max(0, salarioBrutoAnual - reduccionDiscapacidad);

            // 4. Determinar porcentaje según tramos y situación familiar
            decimal porcentajeIRPF = 0;

            if (baseCalculo < 12450)
            {
                porcentajeIRPF = situacionFamiliar switch
                {
                    "S1" => 9.5m,
                    "S2" => 7.5m,
                    "M1" => 8.5m,
                    "M2" => 6.5m,
                    "M3" => 5.5m,
                    _ => 9.5m
                };
            }
            else if (baseCalculo < 20200)
            {
                porcentajeIRPF = situacionFamiliar switch
                {
                    "S1" => 12.0m,
                    "S2" => 10.0m,
                    "M1" => 11.0m,
                    "M2" => 9.0m,
                    "M3" => 8.0m,
                    _ => 12.0m
                };
            }
            else if (baseCalculo < 35200)
            {
                porcentajeIRPF = situacionFamiliar switch
                {
                    "S1" => 15.0m,
                    "S2" => 13.0m,
                    "M1" => 14.0m,
                    "M2" => 12.0m,
                    "M3" => 11.0m,
                    _ => 15.0m
                };
            }
            else if (baseCalculo < 60000)
            {
                porcentajeIRPF = situacionFamiliar switch
                {
                    "S1" => 18.5m,
                    "S2" => 16.5m,
                    "M1" => 17.5m,
                    "M2" => 15.5m,
                    "M3" => 14.5m,
                    _ => 18.5m
                };
            }
            else
            {
                porcentajeIRPF = situacionFamiliar switch
                {
                    "S1" => 22.5m,
                    "S2" => 20.5m,
                    "M1" => 21.5m,
                    "M2" => 19.5m,
                    "M3" => 18.5m,
                    _ => 22.5m
                };
            }

            // 5. Ajuste mínimo del 2% y máximo del 40%
            return Math.Min(40m, Math.Max(2m, porcentajeIRPF));
        }

    }
}
