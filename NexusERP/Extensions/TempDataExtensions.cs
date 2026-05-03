using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NexusERP.Models.UI;
using System.Text.Json;

namespace NexusERP.Extensions
{
    public static class TempDataExtensions
    {
        private const string ALERTKEY = "ALERT_MESSAGE";

        public static void SetAlert(this ITempDataDictionary tempData, AlertMessage alert)
        {
            tempData[ALERTKEY] = JsonSerializer.Serialize(alert);
        }

        public static AlertMessage GetAlert(this ITempDataDictionary tempData)
        {
            if (tempData.ContainsKey(ALERTKEY))
            {
                return JsonSerializer.Deserialize<AlertMessage>((string)tempData[ALERTKEY]);
            }

            return null;
        }
    }
}
