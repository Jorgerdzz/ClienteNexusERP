using ApiNexusERP.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NexusERP.Enums;
using NexusERP.Models;
using NexusERP.Services;
using NexusERP.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;

namespace NexusERP.Controllers
{
    [Authorize(Policy = "ADMIN")]
    public class EmpleadosController : Controller
    {
        private ServiceEmpleados serviceEmpleados;
        private ServiceDepartamentos serviceDepartamentos;

        public EmpleadosController(ServiceEmpleados serviceEmpleados, ServiceDepartamentos serviceDepartamentos)
        {
            this.serviceEmpleados = serviceEmpleados;
            this.serviceDepartamentos = serviceDepartamentos;
        }

        public async Task<IActionResult> Index(int? idDepartamento)
        {
            EmpleadosViewModel model = new EmpleadosViewModel();
            var departamentosAPI = await this.serviceDepartamentos.GetDepartamentosAsync();
            model.Departamentos = departamentosAPI.Select(d => new Departamento { Id = d.Id, Nombre = d.Nombre }).ToList();

            List<EmpleadoDTO> empleadosDTO;
            if (idDepartamento != null)
            {
                empleadosDTO = await this.serviceEmpleados.GetEmpleadosDepartamentoAsync(idDepartamento.Value);
                ViewBag.DepartamentoSeleccionado = idDepartamento.Value;
            }
            else
            {
                empleadosDTO = await this.serviceEmpleados.GetEmpleadosAsync();
            }

            model.Empleados = empleadosDTO.Select(e => new Empleado
            {
                Id = e.Id,
                DepartamentoId = e.DepartamentoId,
                Departamento = new Departamento { Nombre = e.NombreDepartamento },
                Nombre = e.Nombre,
                Apellidos = e.Apellidos,
                EmailCorporativo = e.EmailCorporativo,
                Telefono = e.Telefono,
                SalarioBrutoAnual = e.SalarioBrutoAnual,
                Activo = e.Activo
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            EmpleadoDTO e = await this.serviceEmpleados.FindEmpleadoAsync(id);
            if (e == null) return NotFound();

            Empleado empleado = new Empleado
            {
                Id = e.Id,
                DepartamentoId = e.DepartamentoId,
                Departamento = new Departamento { Nombre = e.NombreDepartamento },
                Nombre = e.Nombre,
                Apellidos = e.Apellidos,
                Dni = e.Dni,
                EmailCorporativo = e.EmailCorporativo,
                Telefono = e.Telefono,
                SalarioBrutoAnual = e.SalarioBrutoAnual,
                Iban = e.IbanEnmascarado ?? e.Iban,
                Activo = e.Activo
                // Add more mapped properties as necessary depending on your View
            };

            return View(empleado);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmpleadoViewModel model)
        {
            if (ModelState.IsValid)
            {
                EmpleadoDTO emp = new EmpleadoDTO
                {
                    DepartamentoId = model.DepartamentoId,
                    Nombre = model.Nombre,
                    Apellidos = model.Apellidos,
                    Dni = model.DNI,
                    EmailCorporativo = model.EmailCorporativo,
                    Telefono = model.Telefono,
                    SalarioBrutoAnual = model.SalarioBrutoAnual,
                    Iban = model.IBAN?.Replace(" ", "").ToUpper(),
                    Activo = (model.Activo == EstadoEmpleado.Activo)
                };

                var exito = await this.serviceEmpleados.CreateEmpleadoAsync(emp);

                if (exito != null)
                {
                    AlertService.Success(TempData, $"El empleado {model.Nombre} se ha añadido correctamente a la plantilla.");
                    return RedirectToAction("Details", "Departaments", new { id = model.DepartamentoId });
                }
                else
                {
                    AlertService.Error(TempData, "Ocurrió un error inesperado al guardar en la base de datos.");
                }

            }
            else
            {
                AlertService.Warning(TempData, "Revisa los datos del formulario.Algunos campos no tienen el formato correcto.");
                return RedirectToAction("Details", "Departaments", new { id = model.DepartamentoId });
            }
            return RedirectToAction("Details", "Departaments", new { id = model.DepartamentoId });
        }

        [HttpPost]
        public async Task<IActionResult> ImportCsv(IFormFile file, int departamentoId)
        {
            if (file == null || file.Length == 0)
            {
                AlertService.Error(TempData, "Debes seleccionar un archivo CSV.");
                return RedirectToAction("Details", "Departaments", new { id = departamentoId });
            }

            int empleadosImportados = 0;
            List<string> errores = new List<string>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                int fila = 0;

                while (!reader.EndOfStream)
                {
                    var linea = await reader.ReadLineAsync();
                    fila++;

                    // 1. Saltar cabecera
                    if (fila == 1) continue;

                    // 2. PROTECCIÓN 1: Ignorar líneas fantasma vacías de Excel
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    // 3. PROTECCIÓN 2: Separador inteligente (Coma o Punto y Coma)
                    var valores = linea.Contains(';') ? linea.Split(';') : linea.Split(',');

                    // 4. PROTECCIÓN 3: Si la línea solo tiene ;;;;;; la ignoramos
                    if (valores.Length < 13) continue;

                    try
                    {
                        var model = new CreateEmpleadoViewModel
                        {
                            DepartamentoId = departamentoId,
                            Nombre = valores[0],
                            Apellidos = valores[1],
                            DNI = valores[2],
                            EmailCorporativo = valores[3],
                            FechaNacimiento = DateOnly.Parse(valores[4]),
                            NumSeguridadSocial = valores[5],
                            FechaAntiguedad = DateOnly.Parse(valores[6]),
                            GrupoCotizacion = (GrupoCotizacion)int.Parse(valores[7]),
                            SalarioBrutoAnual = decimal.Parse(valores[8]),
                            IBAN = valores[9],
                            // PROTECCIÓN 4: Leemos el número directamente, igual que en el global
                            EstadoCivil = GetEstadoCivil(valores[10]),
                            NumeroHijos = int.Parse(valores[11]),
                            PorcentajeDiscapacidad = int.Parse(valores[12])
                        };

                        var context = new ValidationContext(model, null, null);
                        var results = new List<ValidationResult>();

                        if (!Validator.TryValidateObject(model, context, results, true))
                        {
                            errores.Add($"Fila {fila}: Faltan datos obligatorios o el formato es incorrecto.");
                            continue;
                        }

                        var empleado = new EmpleadoDTO
                        {
                            DepartamentoId = model.DepartamentoId,

                            Nombre = model.Nombre,
                            Apellidos = model.Apellidos,
                            Dni = model.DNI,
                            EmailCorporativo = model.EmailCorporativo,

                            FechaNacimiento = model.FechaNacimiento,
                            FechaAntiguedad = model.FechaAntiguedad,

                            NumSeguridadSocial = model.NumSeguridadSocial,

                            GrupoCotizacion = (int)model.GrupoCotizacion,

                            EstadoCivil = (int)model.EstadoCivil,
                            NumeroHijos = model.NumeroHijos,
                            PorcentajeDiscapacidad = model.PorcentajeDiscapacidad,

                            SalarioBrutoAnual = model.SalarioBrutoAnual,
                            Iban = model.IBAN?.Replace(" ", "").ToUpper(),
                            Activo = true
                        };

                        var exito = await serviceEmpleados.CreateEmpleadoAsync(empleado);

                        if (exito != null)
                        {
                            empleadosImportados++;
                        }
                        else
                        {
                            errores.Add($"Fila {fila}: El DNI o Email ya existe en la base de datos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        errores.Add($"Fila {fila}: Error de formato. {ex.Message}");
                    }
                }
            }

            // 5. RESPUESTA AL USUARIO
            if (errores.Any())
            {
                // Te muestro el primer error concreto para que sepas por qué ha fallado si vuelve a pasar
                AlertService.Warning(TempData, $"Se importaron {empleadosImportados}. Falló alguna fila: {errores.First()}");
            }
            else
            {
                AlertService.Success(TempData, $"Se importaron {empleadosImportados} empleados correctamente.");
            }

            return RedirectToAction("Details", "Departaments", new { id = departamentoId });
        }

        [HttpGet]
        public IActionResult DescargarPlantillaCsv(int departamentoId)
        {
            var csv = new StringBuilder();

            csv.AppendLine("Nombre,Apellidos,DNI,EmailCorporativo,FechaNacimiento,NumSeguridadSocial,FechaAntiguedad,GrupoCotizacion,SalarioBrutoAnual,IBAN,EstadoCivil,NumeroHijos,PorcentajeDiscapacidad");

            csv.AppendLine("Juan,Perez,12345678A,juan@empresa.com,1990-05-10,123456789012,2022-01-01,5,25000,ES7620770024003102575766,soltero,0,0");

            return File(
                Encoding.UTF8.GetBytes(csv.ToString()),
                "text/csv",
                "PlantillaEmpleados.csv");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool eliminado = await this.serviceEmpleados.DeleteEmpleadoAsync(id);
            if (eliminado)
            {
                AlertService.Success(TempData, "El empleado ha sido eliminado correctamente.");
                return Ok();
            }
            else
            {
                return BadRequest("No se pudo eliminar. Es posible que el empleado tenga nóminas o datos contables asociados.");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditEmpleadoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Extraemos el nombre exacto de los campos que están fallando
                var camposConError = ModelState.Where(ms => ms.Value.Errors.Any())
                                               .Select(ms => ms.Key)
                                               .ToList();

                string mensajeErrores = string.Join(", ", camposConError);

                // Hacemos que el SweetAlert nos chive exactamente qué falla
                AlertService.Warning(TempData, $"Fallo de validación en: {mensajeErrores}");
                return RedirectToAction("Index");
            }

            try
            {
                EmpleadoDTO emp = await this.serviceEmpleados.FindEmpleadoAsync(model.Id);

                if (emp == null)
                {
                    AlertService.Error(TempData, "El empleado que intentas modificar no existe.");
                    return RedirectToAction("Index");
                }

                emp.Nombre = model.Nombre;
                emp.Apellidos = model.Apellidos;
                emp.Dni = model.DNI;
                emp.EmailCorporativo = model.EmailCorporativo;
                emp.Telefono = model.Telefono;

                emp.DepartamentoId = model.DepartamentoId;
                emp.SalarioBrutoAnual = Convert.ToDecimal(model.SalarioBrutoAnual);
                emp.Iban = model.IBAN;
                emp.Activo = model.Activo;

                await this.serviceEmpleados.UpdateEmpleadoAsync(emp);
                AlertService.Toast(TempData, "Datos del empleado actualizados correctamente", "success");

            }
            catch
            {
                AlertService.Error(TempData, "Hubo un error al guardar los cambios en la base de datos.");
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult DescargarPlantillaCsvGlobal()
        {
            var csv = new StringBuilder();
            // Añadimos "Departamento" como primera columna
            csv.AppendLine("Departamento,Nombre,Apellidos,DNI,EmailCorporativo,FechaNacimiento,NumSeguridadSocial,FechaAntiguedad,GrupoCotizacion,SalarioBrutoAnual,IBAN,EstadoCivil,NumeroHijos,PorcentajeDiscapacidad");

            // Fila de ejemplo
            csv.AppendLine("Recursos Humanos,Juan,Perez,12345678A,juan@empresa.com,1990-05-10,123456789012,2022-01-01,5,25000,ES7620770024003102575766,soltero,0,0");

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "PlantillaGlobalEmpleados.csv");
        }


        private int? BuscarDepartamentoInteligente(string nombreCsv, List<DepartamentoDTO> departamentosBD)
        {
            if (string.IsNullOrWhiteSpace(nombreCsv)) return null;

            string LimpiarTexto(string texto)
            {
                var normalizedString = texto.Normalize(NormalizationForm.FormD);
                var stringBuilder = new StringBuilder();
                foreach (var c in normalizedString)
                {
                    var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                    if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                    {
                        stringBuilder.Append(c);
                    }
                }
                return stringBuilder.ToString().Normalize(NormalizationForm.FormC).ToLower().Trim();
            }

            string textoLimpio = LimpiarTexto(nombreCsv);

            // Diccionario de Alias 
            var alias = new Dictionary<string, string>
        {
            { "rrhh", "recursos humanos" },
            { "it", "informatica" },
            { "sistemas", "informatica" },
            { "admin", "administracion" },
            { "mkt", "marketing" }
        };


            if (alias.ContainsKey(textoLimpio))
            {
                textoLimpio = alias[textoLimpio];
            }

            // Buscamos en la Base de Datos comparando los textos limpios
            var dept = departamentosBD.FirstOrDefault(d => LimpiarTexto(d.Nombre) == textoLimpio);

            return dept?.Id;
        }


        [HttpPost]
        public async Task<IActionResult> ImportCsvGlobal(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                AlertService.Error(TempData, "Debes seleccionar un archivo CSV.");
                return RedirectToAction("Index");
            }

            int empleadosImportados = 0;
            List<string> errores = new List<string>();

            var departamentosBD = await this.serviceDepartamentos.GetDepartamentosAsync();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                int fila = 0;
                while (!reader.EndOfStream)
                {
                    var linea = await reader.ReadLineAsync();
                    fila++;

                    // 1. Saltar cabecera
                    if (fila == 1) continue;

                    // 2. PROTECCIÓN 1: Ignorar líneas fantasma vacías de Excel
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    var valores = linea.Contains(';') ? linea.Split(';') : linea.Split(',');

                    if (valores.Length < 14) continue;

                    try
                    {
                        // valores[0] es el nombre del departamento
                        int? idDept = BuscarDepartamentoInteligente(valores[0], departamentosBD);

                        if (idDept == null)
                        {
                            errores.Add($"Fila {fila}: El departamento '{valores[0]}' no existe en la empresa.");
                            continue;
                        }

                        // Llenamos la "maleta" DTO completa para que Azure no rechace la inserción
                        var empleado = new EmpleadoDTO
                        {
                            DepartamentoId = idDept.Value,
                            Nombre = valores[1],
                            Apellidos = valores[2],
                            Dni = valores[3],
                            EmailCorporativo = valores[4],                          
                            FechaNacimiento = DateOnly.Parse(valores[5]),
                            NumSeguridadSocial = valores[6],
                            FechaAntiguedad = DateOnly.Parse(valores[7]),
                            GrupoCotizacion = int.Parse(valores[8]),                           
                            SalarioBrutoAnual = decimal.Parse(valores[9]),
                            Iban = valores[10]?.Replace(" ", "").ToUpper(),
                            EstadoCivil = (int)GetEstadoCivil(valores[11]), 
                            NumeroHijos = int.Parse(valores[12]),
                            PorcentajeDiscapacidad = int.Parse(valores[13]),

                            Activo = true
                        };

                        var exito = await serviceEmpleados.CreateEmpleadoAsync(empleado);

                        if (exito != null)
                        {
                            empleadosImportados++;
                        }
                        else
                        {
                            errores.Add($"Fila {fila}: El DNI '{valores[3]}' o Email ya existe en la base de datos.");
                        }
                    }
                    catch (Exception ex)
                    {
                        errores.Add($"Fila {fila}: Error de formato. Revisa fechas o números. ({ex.Message})");
                    }
                }
            }

            if (errores.Any())
            {
                AlertService.Warning(TempData, $"Se importaron {empleadosImportados}. Falló alguna fila: {errores.First()}");
            }
            else
            {
                AlertService.Toast(TempData, $"Se importaron {empleadosImportados} empleados correctamente.", "success");
            }

            return RedirectToAction("Index");
        }

        private EstadoCivil GetEstadoCivil(string estadoCivilStr)
        {
            return estadoCivilStr.ToLower() switch
            {
                "soltero" => EstadoCivil.Soltero,
                "casado" => EstadoCivil.Casado,
                "divorciado" => EstadoCivil.Divorciado,
                "viudo" => EstadoCivil.Viudo,
                _ => throw new ArgumentException($"Estado Civil desconocido: {estadoCivilStr}")
            };
        }

    }
}
