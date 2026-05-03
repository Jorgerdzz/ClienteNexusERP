using System;

namespace ApiNexusERP.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public int? EmpleadoId { get; set; }
        
        // Relación opcional para cuando se pida el perfil con Empleado incluido
        public EmpleadoDTO Empleado { get; set; }
    }
}
