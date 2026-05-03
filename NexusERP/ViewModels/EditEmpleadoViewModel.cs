using NexusERP.Enums;
using System.ComponentModel.DataAnnotations;

namespace NexusERP.ViewModels
{
    public class EditEmpleadoViewModel
    {
        [Required]
        public int Id { get; set; }

        // ==========================================
        // DATOS PERSONALES
        // ==========================================
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre es demasiado largo.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(100, ErrorMessage = "Los apellidos son demasiado largos.")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El DNI/NIE es obligatorio.")]
        [StringLength(20)]
        public string DNI { get; set; }

        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string EmailCorporativo { get; set; }

        public string Telefono { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateOnly FechaNacimiento { get; set; }

        // ==========================================
        // DATOS LABORALES Y ECONÓMICOS
        // ==========================================
        [Required(ErrorMessage = "Debe asignar un departamento.")]
        public int DepartamentoId { get; set; }

        [Required(ErrorMessage = "El número de la Seguridad Social es obligatorio.")]
        public string NumSeguridadSocial { get; set; }

        [Required(ErrorMessage = "La fecha de alta es obligatoria.")]
        [DataType(DataType.Date)]
        public DateOnly FechaAntiguedad { get; set; }

        [Required(ErrorMessage = "El grupo de cotización es obligatorio.")]
        [Range(1, 11, ErrorMessage = "El grupo de cotización debe estar entre 1 y 11.")]
        public int GrupoCotizacion { get; set; }

        [Required(ErrorMessage = "El salario bruto anual es obligatorio.")]
        public string SalarioBrutoAnual { get; set; }

        public string IBAN { get; set; }

        public bool Activo { get; set; }

        // ==========================================
        // DATOS PARA IRPF
        // ==========================================
        [Required(ErrorMessage = "El estado civil es obligatorio.")]
        public int EstadoCivil { get; set; }

        [Range(0, 20, ErrorMessage = "El número de hijos no tiene un formato válido.")]
        public int NumeroHijos { get; set; }

        [Range(0, 100, ErrorMessage = "El porcentaje de discapacidad debe estar entre 0 y 100.")]
        public int PorcentajeDiscapacidad { get; set; }
    }
}
