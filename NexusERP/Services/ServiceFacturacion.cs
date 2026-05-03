using ApiNexusERP.DTOs;
using NexusERP.DTOs; // Asegúrate de tener copiado el FacturaDTO y sus DTOs hijos (FacturaDetalleDTO)
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceFacturacion
    {
        private ServiceApi serviceApi;

        public ServiceFacturacion(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<List<FacturaDTO>> GetFacturasAsync()
        {
            // Ruta limpia, el controlador original no tiene [action] aquí
            return await this.serviceApi.CallGetAsync<List<FacturaDTO>>("api/Facturas");
        }

        public async Task<FacturaDTO> FindFacturaAsync(int idFactura)
        {
            // La ruta es simplemente api/Facturas/{id}
            return await this.serviceApi.CallGetAsync<FacturaDTO>($"api/Facturas/{idFactura}");
        }

        public async Task<FacturaDTO> EmitirFacturaAsync(FacturaDTO factura)
        {
            // Gracias al [action], la ruta apunta a Emitir. 
            // La API lee el Token, extrae el ID de la Empresa, genera la Factura, el Asiento y los Apuntes.
            return await this.serviceApi.CallPostAsync<FacturaDTO>("api/Facturas/Emitir", factura);
        }

        public async Task<bool> CobrarFacturaAsync(int idFactura)
        {
            try
            {
                // El PUT de tu API solo recibe el ID por la URL, no espera un cuerpo JSON.
                // Como nuestro método genérico CallPutAsync pide un "modelo", le pasamos un objeto anónimo vacío (new {})
                await this.serviceApi.CallPutAsync($"api/Facturas/Cobrar/{idFactura}", new { });
                return true;
            }
            catch
            {
                // Si la API devuelve BadRequest (ej: si ya estaba pagada), capturamos el error y devolvemos false
                return false;
            }
        }
    }
}