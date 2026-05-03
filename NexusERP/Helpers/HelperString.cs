namespace NexusERP.Helpers
{
    public static class HelperString
    {
        public static string ObtenerIniciales(this string nombreCompleto)
        {
            if (string.IsNullOrWhiteSpace(nombreCompleto))
                return "XX";

            var partes = nombreCompleto.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (partes.Length == 1)
                return partes[0].Substring(0, 1).ToUpper();

            return (partes[0].Substring(0, 1) + partes[1].Substring(0, 1)).ToUpper();
        }
    }
}
