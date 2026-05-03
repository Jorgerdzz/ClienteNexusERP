using NexusERP.Enums;
using NexusERP.Helpers;

namespace NexusERP.Models
{
    public class UsuarioSessionModel
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public RolesUsuario Rol { get; set; }
        public int EmpresaId { get; set; }
        public string NombreEmpresa { get; set; }
        public int EmpleadoId { get; set; }
        public string inicialesUsuario => Nombre.ObtenerIniciales();
    }
}
