using System.ComponentModel.DataAnnotations;

namespace NexusERP.ViewModels
{
    public class MiPerfilViewModel
    {
        // --- DATOS EDITABLES (Tabla Usuarios) ---
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email incorrecto")]
        public string Email { get; set; }

        public string? PasswordActual { get; set; }

        [MinLength(6, ErrorMessage = "La nueva contraseña debe tener al menos 6 caracteres")]
        public string? NuevaPassword { get; set; }

        [Compare("NuevaPassword", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmarPassword { get; set; }

        // --- DATOS DE LECTURA (Le añadimos el '?' para que .NET no exija que vengan en el formulario) ---
        public string? NombreEmpresa { get; set; }
        public bool EstaVinculadoAEmpleado { get; set; }

        public string? EmpleadoNombreCompleto { get; set; }
        public string? EmpleadoDNI { get; set; }
        public string? EmpleadoTelefono { get; set; }
        public string? DepartamentoNombre { get; set; }
        public string? FechaAntiguedad { get; set; }
    }
}