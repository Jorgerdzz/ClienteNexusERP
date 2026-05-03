using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusERP.Helpers;
using NexusERP.Models;
using NexusERP.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexusERP.Controllers
{
    [Authorize(Policy = "ADMIN")]
    public class ClientesController : Controller
    {
        private ServiceClientes serviceClientes;
        private HelperSessionContextAccessor contextAccessor;

        public ClientesController(ServiceClientes serviceClientes, HelperSessionContextAccessor contextAccessor)
        {
            this.serviceClientes = serviceClientes;
            this.contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            var clientesAPI = await this.serviceClientes.GetClientesAsync();
            List<Cliente> clientes = clientesAPI.Select(c => new Cliente
            {
                Id = c.Id,
                EmpresaId = c.EmpresaId,
                RazonSocial = c.RazonSocial,
                CifNif = c.CifNif,
                Email = c.Email,
                Activo = c.Activo
            }).ToList();
            
            return View(clientes);
        }

        public async Task<IActionResult> Details(int id)
        {
            var c = await this.serviceClientes.FindClienteAsync(id);
            if (c == null) return NotFound();

            Cliente cliente = new Cliente
            {
                Id = c.Id,
                EmpresaId = c.EmpresaId,
                RazonSocial = c.RazonSocial,
                CifNif = c.CifNif,
                Email = c.Email,
                Activo = c.Activo
            };

            return View(cliente);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string razonSocial, string cifNif, string email)
        {
            int empresaId = this.contextAccessor.GetEmpresaIdSession();

            NugetModelsNexusERP.Models.Cliente nuevo = new NugetModelsNexusERP.Models.Cliente
            {
                EmpresaId = empresaId,
                RazonSocial = razonSocial,
                CifNif = cifNif,
                Email = email,
                Activo = true
            };

            var creado = await this.serviceClientes.CreateClienteAsync(nuevo);

            if (creado != null)
            {
                AlertService.Toast(TempData, "Cliente guardado correctamente", "success");
            }
            else
            {
                AlertService.Error(TempData, "Hubo un error al guardar el cliente");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string razonSocial, string cifNif, string email, int id)
        {
            int empresaId = this.contextAccessor.GetEmpresaIdSession();

            NugetModelsNexusERP.Models.Cliente nuevo = new NugetModelsNexusERP.Models.Cliente
            {
                Id = id,
                EmpresaId = empresaId,
                RazonSocial = razonSocial,
                CifNif = cifNif,
                Email = email,
                Activo = true
            };

            var actualizado = await this.serviceClientes.UpdateClienteAsync(nuevo);

            if (actualizado != null)
            {
                AlertService.Toast(TempData, "Cliente actualizado correctamente", "success");
            }
            else
            {
                AlertService.Error(TempData, "Hubo un error al actualizar el cliente");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool eliminado = await this.serviceClientes.DeleteClienteAsync(id);

            if (eliminado)
            {
                AlertService.Toast(TempData, "Cliente eliminado correctamente", "success");
            }
            else
            {
                AlertService.Error(TempData, "Hubo un error al eliminar el cliente");
            }

            return RedirectToAction("Index");
        }

    }
}
