using System.ComponentModel.DataAnnotations;

namespace NexusERP.ViewModels
{
    public class RegistroViewModel
    {
        // --- DATOS DE LA EMPRESA ---
        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [Display(Name = "Nombre de la Empresa / Razón Social")]
        public string NombreEmpresa { get; set; }

        [Required(ErrorMessage = "El CIF es obligatorio.")]
        [StringLength(20, ErrorMessage = "El CIF no puede superar los 20 caracteres.")]
        public string CIF { get; set; }

        // --- DATOS DEL USUARIO ADMINISTRADOR ---
        [Required(ErrorMessage = "Tu nombre es obligatorio.")]
        [Display(Name = "Tu Nombre Completo")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres.")]
        [DataType(DataType.Password)] 
        public string Password { get; set; }
    }
}