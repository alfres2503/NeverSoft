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
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Maintenance()
        {
            IEnumerable<User> list = null;
            try
            {
                IServiceUser _ServiceUser = new ServiceUser();
                list = _ServiceUser.GetUsers();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                return RedirectToAction("Default", "Error");
            }

            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            return View(list);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IDRole = listRoles();
            return View();
        }


        public ActionResult Edit(long? id)
        {
            IServiceUser _ServiceUser = new ServiceUser();
            User user = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Maintenance");
                }

                user = _ServiceUser.GetUserByID((long)Convert.ToDouble(id));
                if (user == null)
                {
                    TempData["Message"] = "The requested User does not exist";
                    TempData["Redirect"] = "User";
                    TempData["Redirect-Action"] = "Maintenance";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                ViewBag.IDRole = listRoles(user.IDRole);
                return View(user);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                TempData["Redirect"] = "User";
                TempData["Redirect-Action"] = "Maintenance";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [HttpPost]
        public ActionResult SaveCreate(User user)
        {
            IServiceUser _ServiceUser = new ServiceUser();
            try
            {
                if (_ServiceUser.GetUserByID(user.IDUser) != null)
                {
                    ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Error", "ID Already exists", Util.SweetAlertMessageType.error);
                    ViewBag.IDRole = listRoles(); // Esto evita que de el error del dropdown 
                    return View("Create", user);
                }

                if (_ServiceUser.GetUserByEmail(user.Email) != null)
                {
                    ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Error", "Email already Registered", Util.SweetAlertMessageType.error);
                    ViewBag.IDRole = listRoles(); // Esto evita que de el error del dropdown 
                    return View("Create", user);
                }

                if (ModelState.IsValid)
                {
                    User oUser = _ServiceUser.Save(user);
                }
                else
                {
                    ViewBag.IDRole = listRoles(); // Esto evita que de el error del dropdown 
                    return View("Create", user);
                }


                return RedirectToAction("Maintenance");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Maintenance";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [HttpPost]
        public ActionResult SaveEdit(User user)
        {
            IServiceUser _ServiceUser = new ServiceUser();
            try
            {

                if (ModelState.IsValid)
                {
                    User oUser = _ServiceUser.Save(user);
                }
                else
                {
                    return View("Create", user);
                }


                return RedirectToAction("Maintenance");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData["Redirect"] = "Maintenance";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }


        private SelectList listRoles(int idRole = 0)
        {
            IServiceUserRole _ServiceUserRole = new ServiceUserRole();
            IEnumerable<UserRole> lista = _ServiceUserRole.GetUserRoles();

            return new SelectList(lista, "IDRole", "Description", idRole);
        }




    }
}