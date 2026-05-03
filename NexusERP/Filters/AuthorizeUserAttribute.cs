//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc.ViewFeatures;

//namespace NexusERP.Filters
//{
//    public class AuthorizeUserAttribute: AuthorizeAttribute, IAuthorizationFilter
//    {
//        public void OnAuthorization(AuthorizationFilterContext context)
//        {
//            var user = context.HttpContext.User;

//            string controller =
//                context.RouteData.Values["controller"].ToString();

//            string action =
//                context.RouteData.Values["action"].ToString();

//            var id =
//                context.RouteData.Values["idUsuario"];

//            ITempDataProvider provider =
//                context.HttpContext.RequestServices.GetService<ITempDataProvider>();

//            var tempData = provider.LoadTempData(context.HttpContext);

//            tempData["controller"] = controller;
//            tempData["action"] = action;

//            if (id != null)
//            {
//                tempData["idUsuario"] = id.ToString();
//            }
//            else
//            {
//                //ELIMINAMOS LA CLAVE PARA QUE NO SE QUEDE ENTRE PETICIONES
//                tempData.Remove("idUsuario");
//            }

//            provider.SaveTempData(context.HttpContext, tempData);

//            if (user.Identity.IsAuthenticated == false)
//            {
//                context.Result = GetRoute("Account", "LogIn");
//            }
//        }

//        private RedirectToRouteResult GetRoute
//            (string controller, string action)
//        {
//            RouteValueDictionary ruta =
//                new RouteValueDictionary(new
//                {
//                    controller = controller,
//                    action = action
//                });
//            RedirectToRouteResult result =
//                new RedirectToRouteResult(ruta);
//            return result;
//        }

//    }
//}
