using System.ComponentModel.DataAnnotations;

namespace NexusERP.ViewModels
{
    public class EditDepartamentoViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del departamento es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El presupuesto es obligatorio.")]
        [Range(0, 999999999, ErrorMessage = "El presupuesto no puede ser negativo.")]
        public decimal PresupuestoAnual { get; set; }
    }
}
