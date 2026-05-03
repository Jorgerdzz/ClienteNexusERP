using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusERP.Helpers;
using NexusERP.Models;
using NexusERP.Services;
using NexusERP.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;
using ApiNexusERP.DTOs;

namespace NexusERP.Controllers
{
    [Authorize(Policy = "ADMIN")]
    public class AccountingController : Controller
    {
        private ServiceContabilidad serviceContabilidad;
        private HelperSessionContextAccessor contextAccessor;

        public AccountingController(ServiceContabilidad serviceContabilidad, HelperSessionContextAccessor contextAccessor)
        {
            this.serviceContabilidad = serviceContabilidad;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            DashboardFinancieroViewModel modelo = new DashboardFinancieroViewModel();
            List<AsientoContableDTO> libroDiario = await this.serviceContabilidad.GetLibroDiarioAsync();

            var apuntesAnio = libroDiario
                .Where(a => a.Fecha.HasValue && a.Fecha.Value.Year == DateTime.Now.Year)
                .SelectMany(a => a.Apuntes.Select(ap => new { Asiento = a, Apunte = ap }))
                .ToList();

            Dictionary<string,decimal> agrupacionGastos = new Dictionary<string, decimal>();

            foreach (var item in apuntesAnio)
            {
                var apunte = item.Apunte;
                var Asiento = item.Asiento;
                int mesIndex = Asiento.Fecha.Value.Month - 1; // 0 = Ene, 1 = Feb...

                // REGLAS CONTABLES:
                // Grupo 6 (Gastos): Nacen y crecen por el DEBE.
                if (apunte.CuentaCodigo.StartsWith("6"))
                {
                    decimal importeGasto = apunte.Debe - apunte.Haber;
                    if (importeGasto > 0)
                    {
                        modelo.TotalGastos += importeGasto;
                        modelo.GastosMensuales[mesIndex] += importeGasto;

                        
                        if (!agrupacionGastos.ContainsKey(apunte.CuentaNombre))
                            agrupacionGastos[apunte.CuentaNombre] = 0;

                        agrupacionGastos[apunte.CuentaNombre] += importeGasto;
                    }
                }
                // Grupo 7 (Ingresos): Nacen y crecen por el HABER.
                else if (apunte.CuentaCodigo.StartsWith("7"))
                {
                    decimal importeIngreso = apunte.Haber - apunte.Debe;
                    if (importeIngreso > 0)
                    {
                        modelo.TotalIngresos += importeIngreso;
                        modelo.IngresosMensuales[mesIndex] += importeIngreso;
                    }
                }
            }

            // Volcamos el diccionario agrupado a las listas del ViewModel
            foreach (var item in agrupacionGastos)
            {
                modelo.NombresCuentasGasto.Add(item.Key);
                modelo.ImportesGasto.Add(item.Value);
            }

            return View(modelo);

        }

        public async Task<IActionResult> PlanContable()
        {
            List<CuentaContableDTO> cuentas = await this.serviceContabilidad.GetPlanContableAsync();
            return View(cuentas.Select(c => new CuentasContable
            {
                Id = c.Id,
                Codigo = c.Codigo,
                Nombre = c.Nombre,
                Tipo = c.Tipo
            }).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCuenta(string codigo, string nombre, string tipo)
        {
            CuentaContableDTO cuenta = new CuentaContableDTO
            {
                Codigo = codigo,
                Nombre = nombre,
                Tipo = tipo
            };

            var resultado = await this.serviceContabilidad.CrearCuentaContableAsync(cuenta);

            if (resultado != null)
            {
                AlertService.Toast(TempData, "Cuenta creada correctamente.");
            }
            else
            {
                AlertService.Error(TempData, "Error al registrar la cuenta contable.");
            }

            return RedirectToAction("PlanContable"); 
        }

        public async Task<IActionResult> Diario()
        {
            List<AsientoContableDTO> asientos = await this.serviceContabilidad.GetLibroDiarioAsync();
            List<AsientoViewModel> modelo = new List<AsientoViewModel>();

            foreach (var a in asientos)
            {
                AsientoViewModel asiento = new AsientoViewModel
                {
                    Id = a.Id,
                    NumeroAsiento = $"AS-{a.Id:D4}", 
                    Fecha = a.Fecha.Value,
                    Glosa = a.Glosa,
                    Origen = a.Glosa.Contains("Nómina") ? "Nómina" : "Factura" 
                };

                foreach (var apunte in a.Apuntes)
                {
                    asiento.Apuntes.Add(new ApunteViewModel
                    {
                        CuentaCodigo = apunte.CuentaCodigo,
                        CuentaNombre = apunte.CuentaNombre,
                        Debe = apunte.Debe,
                        Haber = apunte.Haber
                    });

                    // Vamos sumando los totales
                    asiento.TotalDebe += apunte.Debe;
                    asiento.TotalHaber += apunte.Haber;
                }

                modelo.Add(asiento);
            }

            return View(modelo);
        }

        // --- FASE 4: LIBRO MAYOR ---
        [HttpGet]
        public async Task<IActionResult> Mayor(int? CuentaIdSeleccionada, DateTime? FechaDesde, DateTime? FechaHasta)
        {
            // Por defecto, mostramos el mes actual si no hay fechas
            DateTime desde = FechaDesde ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime hasta = FechaHasta ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            var modelo = new NexusERP.ViewModels.MayorViewModel
            {
                FechaDesde = desde,
                FechaHasta = hasta,
                CuentaIdSeleccionada = CuentaIdSeleccionada
            };

            // 1. Llenar el desplegable solo con cuentas de detalle (que tengan 3 o más dígitos)
            var cuentasEmpresa = await this.serviceContabilidad.GetPlanContableAsync();
            modelo.CuentasDisponibles = cuentasEmpresa
                .Where(c => c.Codigo.Length >= 3)
                .Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Codigo} - {c.Nombre}"
                }).ToList();

            // 2. Si el usuario ha seleccionado una cuenta, calculamos el Mayor
            if (CuentaIdSeleccionada.HasValue)
            {
                var cuentaDB = cuentasEmpresa.FirstOrDefault(c => c.Id == CuentaIdSeleccionada.Value);
                if (cuentaDB != null)
                {
                    modelo.NombreCuentaSeleccionada = $"{cuentaDB.Codigo} - {cuentaDB.Nombre}";

                    // ¿Cómo suma esta cuenta? (Regla de oro contable)
                    bool sumaPorElDebe = cuentaDB.Tipo == "Activo" || cuentaDB.Tipo == "Gasto";

                    // A. Calcular Saldo Inicial
                    var libroMayor = await this.serviceContabilidad.GetLibroMayorAsync(CuentaIdSeleccionada.Value, desde, hasta);
                    decimal saldoAnteriorBase = libroMayor.SaldoAnterior;
                    decimal saldoAcumulado = sumaPorElDebe ? saldoAnteriorBase : -saldoAnteriorBase;

                    modelo.Movimientos.Add(new NexusERP.ViewModels.MovimientoMayorViewModel
                    {
                        Fecha = desde,
                        AsientoNumero = "-",
                        Concepto = "Saldo Inicial",
                        Debe = 0,
                        Haber = 0,
                        SaldoAcumulado = saldoAcumulado,
                        EsSaldoInicial = true
                    });

                    // B. Obtener asientos generales para vincular fechas a los apuntes devueltos
                    List<AsientoContableDTO> todosLosAsientos = await this.serviceContabilidad.GetLibroDiarioAsync();

                    foreach (var ap in libroMayor.Movimientos)
                    {
                        var asientoPadre = todosLosAsientos.FirstOrDefault(a => a.Apuntes.Any(apunte => apunte.Id == ap.Id));

                        // Actualizar el saldo cronológicamente fila a fila
                        if (sumaPorElDebe)
                            saldoAcumulado += (ap.Debe - ap.Haber);
                        else
                            saldoAcumulado += (ap.Haber - ap.Debe);

                        modelo.Movimientos.Add(new MovimientoMayorViewModel
                        {
                            Fecha = asientoPadre?.Fecha ?? desde,
                            AsientoNumero = asientoPadre != null ? $"AS-{asientoPadre.Id:D4}" : "-",
                            Concepto = asientoPadre?.Glosa ?? "-",
                            Debe = ap.Debe,
                            Haber = ap.Haber,
                            SaldoAcumulado = saldoAcumulado,
                            EsSaldoInicial = false
                        });
                    }

                    modelo.SaldoFinal = saldoAcumulado;
                }
            }

            return View(modelo);
        }
    }
}
