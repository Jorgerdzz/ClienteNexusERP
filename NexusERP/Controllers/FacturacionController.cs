using Microsoft.AspNetCore.Mvc;
using NexusERP.Enums;
using Microsoft.AspNetCore.Authorization;
using NexusERP.Helpers;
using NexusERP.Models;
using NexusERP.ViewModels;
using QuestPDF.Fluent;
using System.Threading.Tasks;
using NexusERP.Services;
using NugetModelsNexusERP.Models;
using ApiNexusERP.DTOs;

namespace NexusERP.Controllers
{
    [Authorize(Policy = "ADMIN")]
    public class FacturacionController : Controller
    {
        private ServiceClientes serviceClientes;
        private ServiceFacturacion serviceFacturacion;
        private HelperSessionContextAccessor contextAccessor;

        public FacturacionController(ServiceClientes serviceClientes, ServiceFacturacion serviceFacturacion, HelperSessionContextAccessor contextAccessor)
        {
            this.serviceClientes = serviceClientes;
            this.serviceFacturacion = serviceFacturacion;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            int idEmpresa = this.contextAccessor.GetEmpresaIdSession();

            DashboardFacturacionViewModel model = new DashboardFacturacionViewModel();

            var clientes = await this.serviceClientes.GetClientesAsync();
            var facturasDTO = await this.serviceFacturacion.GetFacturasAsync();

            model.TotalClientes = clientes.Count(c => c.Activo);
            model.TotalFacturas = facturasDTO.Count;

            model.FacturadoEsteMes = facturasDTO.Where(f => f.FechaEmision.Month == DateTime.Now.Month && f.FechaEmision.Year == DateTime.Now.Year)
                .Sum(f => f.TotalFactura);

            model.PendienteDeCobro = facturasDTO.Where(f => f.Estado == "Pendiente")
                .Sum(f => f.TotalFactura);

            model.UltimasFacturas = facturasDTO.Take(5).Select(f => {
                var clienteDTO = clientes.FirstOrDefault(c => c.Id == f.ClienteId);

                return new NexusERP.Models.Factura
                {
                    Id = f.Id,
                    ClienteId = f.ClienteId,
                    NumeroFactura = f.NumeroFactura,
                    FechaEmision = f.FechaEmision,
                    BaseImponible = f.BaseImponible,
                    IvaTotal = f.IvaTotal,
                    TotalFactura = f.TotalFactura,
                    Estado = f.Estado,
                    EsEmitida = f.EsEmitida,
                    Cliente = clienteDTO != null ? new NexusERP.Models.Cliente
                    {
                        Id = clienteDTO.Id,
                        RazonSocial = clienteDTO.RazonSocial

                    } : new NexusERP.Models.Cliente { RazonSocial = "Desconocido" }
                };
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var f = await this.serviceFacturacion.FindFacturaAsync(id);
            if(f == null) return NotFound();
            
            var factura = new NexusERP.Models.Factura
            {
                Id = f.Id,
                ClienteId = f.ClienteId,
                NumeroFactura = f.NumeroFactura,
                FechaEmision = f.FechaEmision,
                BaseImponible = f.BaseImponible,
                IvaTotal = f.IvaTotal,
                TotalFactura = f.TotalFactura,
                Estado = f.Estado,
                EsEmitida = f.EsEmitida,
                FacturaDetalles = f.Detalles.Select(d => new NexusERP.Models.FacturaDetalle
                {
                    Id = d.Id,
                    Concepto = d.Concepto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    TotalLinea = d.TotalLinea
                }).ToList()
            };

            return View(factura);
        }

        public async Task<IActionResult> NuevaFactura()
        {
            ViewBag.Clientes = await this.serviceClientes.GetClientesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GuardarFactura([FromBody] NuevaFacturaViewModel model)
        {
            int empresaId = this.contextAccessor.GetEmpresaIdSession();

            decimal baseImponible = model.Lineas.Sum(l => l.Cantidad * l.PrecioUnitario);
            decimal ivaTotal = baseImponible * (model.PorcentajeIva / 100m);
            decimal totalFactura = baseImponible + ivaTotal;

            // 2. Montar el DTO Factura
            FacturaDTO facturaDto = new FacturaDTO
            {
                ClienteId = model.ClienteId,
                NumeroFactura = model.NumeroFactura,
                FechaEmision = model.FechaEmision,
                BaseImponible = baseImponible,
                IvaTotal = ivaTotal,
                TotalFactura = totalFactura,
                Estado = "Pendiente",
                EsEmitida = true,
                Detalles = new List<FacturaDetalleDTO>()
            };

            // 3. Montar los detalles
            foreach (var linea in model.Lineas)
            {
                facturaDto.Detalles.Add(new FacturaDetalleDTO
                {
                    Concepto = linea.Concepto,
                    Cantidad = linea.Cantidad,
                    PrecioUnitario = linea.PrecioUnitario,
                    TotalLinea = linea.Cantidad * linea.PrecioUnitario
                });
            }

            // 4. Magia de Guardado y Contabilización
            var res = await this.serviceFacturacion.EmitirFacturaAsync(facturaDto);

            if (res != null)
            {
                string urlDestino = Url.Action("Details", "Facturacion", new { id = res.Id });
                return Json(new { exito = true, urlRedireccion = urlDestino });
            }
            else
            {
                return BadRequest(new { exito = false, mensaje = "Hubo un error al guardar o contabilizar la factura." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarCobro(int idFactura)
        {
            bool resultado = await this.serviceFacturacion.CobrarFacturaAsync(idFactura);
            if (resultado)
            {
                AlertService.Toast(TempData, "Facutra cobrada correctamente.");
            }
            else
            {
                AlertService.Error(TempData, "No se ha podido cobrar la factura.");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DescargarPdf(int idFactura)
        {
            var f = await this.serviceFacturacion.FindFacturaAsync(idFactura);
            if (f == null) return NotFound();

            var factura = new NexusERP.Models.Factura
            {
                Id = f.Id,
                ClienteId = f.ClienteId,
                NumeroFactura = f.NumeroFactura,
                FechaEmision = f.FechaEmision,
                BaseImponible = f.BaseImponible,
                IvaTotal = f.IvaTotal,
                TotalFactura = f.TotalFactura,
                Estado = f.Estado,
                EsEmitida = f.EsEmitida,
                FacturaDetalles = f.Detalles.Select(d => new NexusERP.Models.FacturaDetalle
                {
                    Id = d.Id,
                    Concepto = d.Concepto,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    TotalLinea = d.TotalLinea
                }).ToList()
            };

            var document = new FacturaDocument(factura);
            byte[] pdfBytes = document.GeneratePdf();

            string nombreArchivo = $"Factura_{factura.NumeroFactura}.pdf";

            return File(pdfBytes, "application/pdf", nombreArchivo);
        }

    }

}
