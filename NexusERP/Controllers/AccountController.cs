using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NexusERP.Enums;
using NexusERP.Extensions;
using NexusERP.Helpers;
using NexusERP.Models;
using NexusERP.Models.UI;
using NexusERP.Services;
using NexusERP.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using ApiNexusERP.DTOs;
using NexusERP.DTOs;
using System.Linq;

namespace NexusERP.Controllers
{
    public class AccountController : Controller
    {
        private ServiceAuth serviceAuth;

        public AccountController(ServiceAuth serviceAuth)
        {
            this.serviceAuth = serviceAuth;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistroDTO model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            var resultado = await this.serviceAuth.RegisterUserAsync(model);

            if (resultado)
            {
                return RedirectToAction("LogIn");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error al registrar usuario.");
                return View(model);
            }

        }

        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string token = await this.serviceAuth.LogInUserAsync(model.Email, model.Password);

            if (!string.IsNullOrEmpty(token))
            {
                HttpContext.Session.SetString("TOKEN", token);

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwt = handler.ReadJwtToken(token);

                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name,
                    ClaimTypes.Role
                );
                string nombreUsuario = "Usuario";

                foreach (var c in jwt.Claims)
                {
                    if (c.Type == "role" || c.Type == ClaimTypes.Role)
                    {
                        string nombreRol = c.Value;
                        if (nombreRol == "1") nombreRol = RolesUsuario.Admin.ToString();
                        if (nombreRol == "2") nombreRol = RolesUsuario.Empleado.ToString();

                        identity.AddClaim(new Claim(ClaimTypes.Role, nombreRol));
                    }
                    else if (c.Type == "unique_name" || c.Type == "name" || c.Type == ClaimTypes.Name)
                    {
                        nombreUsuario = c.Value;
                        identity.AddClaim(new Claim(ClaimTypes.Name, c.Value));
                    }
                    else
                    {
                        identity.AddClaim(c);
                    }
                }


                string iniciales = "US"; 
                if (!string.IsNullOrWhiteSpace(nombreUsuario) && nombreUsuario != "Usuario")
                {
                    var partesNombre = nombreUsuario.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (partesNombre.Length >= 2)
                    {
                        iniciales = $"{partesNombre[0][0]}{partesNombre[1][0]}".ToUpper();
                    }
                    else if (partesNombre.Length == 1)
                    {
                        iniciales = partesNombre[0].Substring(0, Math.Min(2, partesNombre[0].Length)).ToUpper();
                    }
                }

                identity.AddClaim(new Claim("Iniciales", iniciales));

                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);


                if (userPrincipal.IsInRole(RolesUsuario.Admin.ToString()))
                {
                    AlertService.Toast(TempData, $"Bienvenido, {nombreUsuario}");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    AlertService.Toast(TempData, $"Bienvenido, {nombreUsuario}");
                    return RedirectToAction("Index", "PortalEmpleado");
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
                return View(model);
            }
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
