using System.ComponentModel.DataAnnotations;

namespace NexusERP.ViewModels
{
    public class SettingsViewModel
    {
        public int EmpresaId { get; set; }

        [Required(ErrorMessage = "El Nombre Comercial es obligatorio.")]
        [StringLength(150)]
        public string NombreComercial { get; set; }

        [Required(ErrorMessage = "La Razón Social es obligatoria.")]
        [StringLength(150)]
        public string RazonSocial { get; set; }

        [Required(ErrorMessage = "El CIF/NIF es obligatorio.")]
        [StringLength(20)]
        public string CIF { get; set; }

        // Datos de solo lectura (Añadimos el '?' para evitar fallos de validación)
        public string? FechaAlta { get; set; }
        public bool? Activo { get; set; }
    }
}
