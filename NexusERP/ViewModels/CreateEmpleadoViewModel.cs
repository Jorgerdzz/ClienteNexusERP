using NexusERP.Enums;
using System.ComponentModel.DataAnnotations;

namespace NexusERP.ViewModels
{
    public class CreateEmpleadoViewModel
    {
        [Required(ErrorMessage = "El departamento es obligatorio.")]
        public int DepartamentoId { get; set; }

        // --- DATOS PERSONALES ---

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(150, ErrorMessage = "Los apellidos no pueden exceder los 150 caracteres.")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El DNI/NIE es obligatorio.")]
        [StringLength(20, ErrorMessage = "El DNI/NIE no puede exceder los 20 caracteres.")]
        [RegularExpression(@"^[XYZ]?\d{5,8}[A-Z]$", ErrorMessage = "El formato del DNI/NIE no es válido (ej: 12345678A o X1234567A).")]
        public string DNI { get; set; }

        [StringLength(150, ErrorMessage = "El email no puede exceder los 150 caracteres.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string EmailCorporativo { get; set; }

        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres.")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido.")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateOnly FechaNacimiento { get; set; }

        // --- DATOS PROFESIONALES Y NÓMINA ---

        [Required(ErrorMessage = "El Nº de Seguridad Social es obligatorio.")]
        [StringLength(20, ErrorMessage = "El NSS no puede exceder los 20 caracteres.")]
        [RegularExpression(@"^\d{12}$", ErrorMessage = "El Número de la Seguridad Social debe tener 12 dígitos.")]
        public string NumSeguridadSocial { get; set; }

        [Required(ErrorMessage = "La fecha de antigüedad es obligatoria.")]
        [DataType(DataType.Date)]
        public DateOnly FechaAntiguedad { get; set; }

        [Required(ErrorMessage = "El grupo de cotización es obligatorio.")]
        [Range(1, 11, ErrorMessage = "El grupo de cotización debe ser un número entre 1 y 11.")]
        public GrupoCotizacion GrupoCotizacion { get; set; }

        [Required(ErrorMessage = "El salario bruto anual es obligatorio.")]
        [Range(0.01, 1000000.00, ErrorMessage = "El salario debe ser mayor que 0 y razonable.")]
        [DataType(DataType.Currency)]
        public decimal SalarioBrutoAnual { get; set; }

        [StringLength(34, ErrorMessage = "El IBAN no puede exceder los 34 caracteres.")]
        [RegularExpression(@"^[A-Z]{2}\d{2}[A-Z0-9]{1,30}$", ErrorMessage = "El formato del IBAN no es válido (ej: ES1234...).")]
        public string IBAN { get; set; }

        // --- DATOS IRPF ---

        [Range(0, 20, ErrorMessage = "El número de hijos debe ser entre 0 y 20.")]
        public int NumeroHijos { get; set; } = 0;

        [Range(0, 100, ErrorMessage = "El porcentaje de discapacidad debe estar entre 0 y 100.")]
        public int PorcentajeDiscapacidad { get; set; } = 0;

        [Required(ErrorMessage = "El estado civil es obligatorio.")]
        [Range(1, 4, ErrorMessage = "Seleccione un estado civil válido.")]
        public EstadoCivil EstadoCivil { get; set; } 

        public EstadoEmpleado Activo { get; set; } = EstadoEmpleado.Activo;
    }
}
