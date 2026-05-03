using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public int EmpresaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int Rol { get; set; }

    public int? EmpleadoId { get; set; }

    public bool? Activo { get; set; }

    public string? Password { get; set; }

    public virtual Empleado? Empleado { get; set; }

    public virtual Empresa Empresa { get; set; } = null!;

    public virtual ICollection<SeguridadUsuario> SeguridadUsuarios { get; set; } = new List<SeguridadUsuario>();
}
