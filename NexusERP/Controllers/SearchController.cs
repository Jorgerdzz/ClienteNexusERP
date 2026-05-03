using Microsoft.AspNetCore.Mvc;
using NexusERP.DTOs;
using NexusERP.Models;
using NexusERP.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace NexusERP.Controllers
{
    public class SearchController : Controller
    {
        private ServiceBusqueda serviceBusqueda;

        public SearchController(ServiceBusqueda serviceBusqueda)
        {
            this.serviceBusqueda = serviceBusqueda;
        }

        public async Task<IActionResult> GlobalSearch(string q)
        {
            if (string.IsNullOrWhiteSpace(q) || q.Length < 2)
            {
                return Json(new List<SearchResultDto>()); 
            }

            string query = q.ToLower().Trim();

            List<SearchResultDto> resultados = await this.serviceBusqueda.BuscarGlobalAsync(query);

            return Json(resultados.OrderBy(r => r.Categoria));
        }
    }
}
