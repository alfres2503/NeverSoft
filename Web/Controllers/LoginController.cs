using ApplicationCore.Services;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Utils;


namespace Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

      

        
        [HttpPost]
        public ActionResult Login(User user)
        {
            IServiceUser _ServiceUser = new ServiceUser();
            User oUser = null;
            try
            {
                ModelState.Remove("FirstName");
                ModelState.Remove("LastName");
                ModelState.Remove("IDRole");
                ModelState.Remove("IDUser");

                if (ModelState.IsValid)
                {
                    oUser = _ServiceUser.GetUsersForLogin(user.Email, user.Password);
                    if (oUser != null)
                    {
                        Session["User"] = oUser;
                        Log.Info($"Access {oUser.Email}");
                        TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Login", "Authenticated user", Util.SweetAlertMessageType.success);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Log.Warn($"Intento de inicio {user.Email}");
                        ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Login", "Invalid User", Util.SweetAlertMessageType.warning);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
            return View("Index");
        }

        public ActionResult UnAuthorized()
        {
            ViewBag.Message = "Not authorized";
            if (Session["User"] != null)
            {
                User usuario = Session["User"] as User;
                Log.Warn($"Not authorized {usuario.Email}");
            }
            return View();
        }
        public ActionResult Logout()
        {
            try
            {
                //Eliminar variable de sesion
                Session["User"] = null;
                Session.Remove("User");
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

       

       
    }
}
