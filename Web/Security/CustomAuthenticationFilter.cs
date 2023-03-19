using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace Web.Security
{
    public class CustomAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        //Método para autenticar la solicitud
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            if ((User)filterContext.HttpContext.Session["User"] == null)
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }
        //Método que se llama cuando falla la autenticación o autorización y 
        //se llama después del método de ejecución de acción, pero antes de renderizar la vista.
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                // Redirija al Controller Login
                filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary
                {
                 { "controller", "Login" },
                 { "action", "Index" }
                 });
            }
        }
    }
}