using NexusERP.Helpers;

namespace NexusERP.ViewModels
{
    public class EmpleadoNominaRowViewModel
    {
        public int EmpleadoId { get; set; }
        public string NombreCompleto { get; set; }
        public string Email { get; set; }
        public string DepartamentoNombre { get; set; }
        public decimal SalarioBrutoAnual { get; set; }
        public bool EstaCalculada { get; set; }
        public int? NominaId { get; set; } 
        public decimal? LiquidoAPercibir { get; set; }
        public string Iniciales => NombreCompleto.ObtenerIniciales();
        public string Estado { get; set; }

    }
}
