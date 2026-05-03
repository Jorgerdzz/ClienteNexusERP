using System.ComponentModel.DataAnnotations;

namespace NexusERP.ViewModels
{
    public class CreateDepartamentoViewModel
    {
        [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El presupuesto es obligatorio.")]
        [Range(0, 9999999.99, ErrorMessage = "El presupuesto debe ser un valor positivo.")]
        public decimal PresupuestoAnual { get; set; }
    }
}
