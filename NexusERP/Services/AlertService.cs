using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NexusERP.Models.UI;
using NexusERP.Extensions;

namespace NexusERP.Services
{
    public static class AlertService
    {
        public static void Success(ITempDataDictionary tempData, string message, string title = "¡Éxito!")
        {
            tempData.SetAlert(new AlertMessage
            {
                Type = "success",
                Title = title,
                Text = message
            });
        }

        public static void Error(ITempDataDictionary tempData, string message, string title = "¡Error!")
        {
            tempData.SetAlert(new AlertMessage
            {
                Type = "error",
                Title = title,
                Text = message
            });
        }

        public static void Warning(ITempDataDictionary tempData, string message, string title = "¡Atención!")
        {
            tempData.SetAlert(new AlertMessage
            {
                Type = "warning",
                Title = title,
                Text = message
            });
        }

        public static void Toast(ITempDataDictionary tempData, string message, string type = "success")
        {
            tempData.SetAlert(new AlertMessage
            {
                Type = type,
                Text = message,
                Toast = true
            });
        }
    }
}
