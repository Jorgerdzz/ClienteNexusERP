using System;
using System.Collections.Generic;

namespace NexusERP.Models;

public partial class SeguridadUsuario
{
    public int IdSeguridad { get; set; }

    public int IdUsuario { get; set; }

    public string Salt { get; set; } = null!;

    public byte[] PasswordHash { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
