using ApiNexusERP.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using NexusERP.Services;
using NexusERP.ViewModels;
using System.Security.Claims;

namespace NexusERP.Controllers
{
    public class ProfileController : Controller
    {
        private ServiceUsuarios serviceUsuarios;

        public ProfileController(ServiceUsuarios serviceUsuarios)
        {
            this.serviceUsuarios = serviceUsuarios;
        }

        public async Task<IActionResult> Index()
        {
            string idUsuarioString = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int idUsuario = int.Parse(idUsuarioString);
            UsuarioDTO user = await this.serviceUsuarios.GetPerfilUsuarioAsync(idUsuario);
            if (user == null) return NotFound();
            MiPerfilViewModel model = new MiPerfilViewModel
            {
                UsuarioId = user.Id,
                NombreUsuario = user.Nombre,
                Email = user.Email,
                NombreEmpresa = "Empresa Actual", // Valor extraído de otro origen si es necesario
                EstaVinculadoAEmpleado = user.EmpleadoId.HasValue
            };
            if (model.EstaVinculadoAEmpleado && user.Empleado != null)
            {
                model.EmpleadoNombreCompleto = $"{user.Empleado.Nombre} {user.Empleado.Apellidos}";
                model.EmpleadoDNI = user.Empleado.Dni;
                model.EmpleadoTelefono = user.Empleado.Telefono;
                model.DepartamentoNombre = user.Empleado.NombreDepartamento ?? "Sin asignar";
                // model.FechaAntiguedad = "Dato omitido en DTO simplificado";
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateInfo(MiPerfilViewModel model)
        {
            ModelState.Remove("PasswordActual");
            ModelState.Remove("NuevaPassword");
            ModelState.Remove("ConfirmarPassword");

            if (!ModelState.IsValid)
            {
                AlertService.Warning(TempData, "Revisa los datos introducidos.");
                return RedirectToAction("Index");
            }

            // Usamos el ID del formulario, pero lo ideal es el de la Cookie
            var usuarioActualizado = await this.serviceUsuarios.UpdatePerfilUsuarioAsync(model.UsuarioId, model.NombreUsuario, model.Email);

            if (usuarioActualizado != null)
            {
                // 🍪 AQUÍ ESTÁ LA MAGIA: REESCRIBIR LA COOKIE EN TIEMPO REAL
                var identity = (ClaimsIdentity)User.Identity;

                var claimNombreAntiguo = identity.FindFirst(System.Security.Claims.ClaimTypes.Name);
                var claimEmailAntiguo = identity.FindFirst(System.Security.Claims.ClaimTypes.Email);
                var claimInicialesAntiguo = identity.FindFirst("Iniciales");

                if (claimNombreAntiguo != null) identity.RemoveClaim(claimNombreAntiguo);
                if (claimEmailAntiguo != null) identity.RemoveClaim(claimEmailAntiguo);
                if (claimInicialesAntiguo != null) identity.RemoveClaim(claimInicialesAntiguo);

                // Ponemos el nuevo nombre y el nuevo email
                identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, model.NombreUsuario));
                identity.AddClaim(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, model.Email));

                // 🎨 RECALCULAMOS LAS INICIALES PARA QUE EL AVATAR CAMBIE AL INSTANTE
                string iniciales = "US";
                if (!string.IsNullOrWhiteSpace(model.NombreUsuario))
                {
                    var partesNombre = model.NombreUsuario.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (partesNombre.Length >= 2)
                        iniciales = $"{partesNombre[0][0]}{partesNombre[1][0]}".ToUpper();
                    else if (partesNombre.Length == 1)
                        iniciales = partesNombre[0].Substring(0, Math.Min(2, partesNombre[0].Length)).ToUpper();
                }
                identity.AddClaim(new System.Security.Claims.Claim("Iniciales", iniciales));

                // Guardamos la Cookie modificada
                await HttpContext.SignInAsync(
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    new System.Security.Claims.ClaimsPrincipal(identity));

                AlertService.Toast(TempData, "Perfil actualizado correctamente", "success");
            }
            else
            {
                AlertService.Error(TempData, "Hubo un error al guardar o el email ya existe.");
            }

            return RedirectToAction("Index");
        }
    }
}
