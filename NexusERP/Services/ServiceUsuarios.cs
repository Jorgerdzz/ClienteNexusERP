using ApiNexusERP.DTOs;
using NexusERP.DTOs; // Asegúrate de tener copiado el UsuarioDTO
using System.Threading.Tasks;

namespace NexusERP.Services
{
    public class ServiceUsuarios
    {
        private ServiceApi serviceApi;

        public ServiceUsuarios(ServiceApi serviceApi)
        {
            this.serviceApi = serviceApi;
        }

        public async Task<UsuarioDTO> FindUsuarioAsync(int id)
        {
            // Ruta estándar apuntando al ID
            return await this.serviceApi.CallGetAsync<UsuarioDTO>($"api/Usuarios/{id}");
        }

        public async Task<UsuarioDTO> GetPerfilUsuarioAsync(int id)
        {
            // Ruta usando el [action] definido en tu API
            return await this.serviceApi.CallGetAsync<UsuarioDTO>($"api/Usuarios/Perfil/{id}");
        }

        public async Task<UsuarioDTO> UpdatePerfilUsuarioAsync(int idUsuario, string nombre, string email)
        {
            // Creamos un DTO parcial solo con los datos que la API exige actualizar
            var updateData = new UsuarioDTO
            {
                Nombre = nombre,
                Email = email
            };

            // Tu endpoint PUT envía el ID por URL y recibe el objeto por Body
            // Al devolver el DTO actualizado (o null si falla), tu controlador MVC sabrá si hubo éxito
            try
            {
                // Como nuestro CallPutAsync genérico no devuelve datos, podemos adaptar la llamada,
                // o bien usar un pequeño truco: hacer el PUT y luego volver a pedir el Perfil actualizado.
                // Asumiendo la estructura actual de nuestro ServiceApi, hacemos el PUT:
                await this.serviceApi.CallPutAsync($"api/Usuarios/UpdatePerfil/{idUsuario}", updateData);

                // Si no ha saltado ninguna excepción (BadRequest), pedimos el perfil renovado:
                return await this.GetPerfilUsuarioAsync(idUsuario);
            }
            catch
            {
                // Si el email ya existía o el usuario no es válido, la API devolverá BadRequest y caerá aquí
                return null;
            }
        }
    }
}