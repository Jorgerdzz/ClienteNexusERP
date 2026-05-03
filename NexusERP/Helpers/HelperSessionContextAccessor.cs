using NexusERP.Extensions;
using NexusERP.Models;

namespace NexusERP.Helpers
{
    public class HelperSessionContextAccessor
    {
        private IHttpContextAccessor contextAccessor;

        public HelperSessionContextAccessor(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        public int GetEmpresaIdSession()
        {
            if (this.contextAccessor.HttpContext?.User == null)
            {
                return 0;
            }

            var claimEmpresaId = this.contextAccessor.HttpContext.User.FindFirst("EmpresaId");

            // Si existe y es un número válido, lo devolvemos
            if (claimEmpresaId != null && int.TryParse(claimEmpresaId.Value, out int empresaId))
            {
                return empresaId;
            }

            return 0;

        }

    }
}
