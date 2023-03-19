using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;


namespace Web.Security
{
    public class AutorizeView
    {
        public static bool IsUserInRole(string[] nombreRoles)
        {
            IEnumerable<UserRoles> allowedroles = nombreRoles.
                Select(a => (UserRoles)Enum.Parse(typeof(UserRoles), a));
            bool authorize = false;
            var oUsuario = (User)HttpContext.Current.Session["User"];
            if (oUsuario != null)
            {
                foreach (var rol in allowedroles)
                {
                    if ((int)rol == oUsuario.IDRole)
                        return true;
                }
            }
            return authorize;
        }
    }
}