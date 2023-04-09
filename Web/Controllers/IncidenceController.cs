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
    public class IncidenceController : Controller
    {
        // GET: Incidence
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult Maintenance()
        {
            IEnumerable<Incidence> list = null;
            try
            {
                IServiceIncidence _ServiceIncidence = new ServiceIncidence();
                list = _ServiceIncidence.GetIncidences();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult Create()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            ViewBag.IDUser = listUsers(GetSessionUser().IDUser);
            return View();
        }

        public ActionResult Edit(int? id)
        {
            IServiceIncidence _ServiceIncidence = new ServiceIncidence();
            Incidence oIncidence = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Maintenance");
                }
                oIncidence = _ServiceIncidence.GetIncidenceByID(Convert.ToInt32(id));
                if (oIncidence == null)
                {
                    TempData["Message"] = "The requested Incidence does not exist";
                    //Controlador
                    TempData["Redirect"] = "Incidence";
                    //Acción
                    TempData["Redirect-Action"] = "Maintenance";
                    return RedirectToAction("Default", "Error");
                }
                //ViewBag.IDUser = listUsers(oIncidence.IDUser);
                ViewBag.IDUser = listUserByID(oIncidence.IDUser);
                return View(oIncidence);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
        }

        private User GetSessionUser()
        {
            User oUser = null;
            //Validar si existe la Session
            if (Session["user"] != null)
            {
                oUser = (User)Session["User"];
            }
            return oUser;
        }

        private SelectList listUsers(long idUser = 0)
        {
            IServiceUser _ServiceUser = new ServiceUser();
            IEnumerable<User> lista = _ServiceUser.GetUsers()
                //.Where(u => u.Active == true && u.IDRole == 2);
                .Where(u => u.IDUser == idUser);


            return new SelectList(lista, "IDUser", "FullName", idUser);
        }



        private MultiSelectList listUserByID(long id, ICollection<User> users = null)
        {
            IServiceUser _ServiceUser = new ServiceUser();
            IEnumerable<User> lista = _ServiceUser.GetUsers()
            .Where(u => u.IDUser == id);
           
            long[] listUserSelect = null;
            if (users != null)
            {
                listUserSelect = users.Select(c => c.IDUser).ToArray();
            }

            return new MultiSelectList(lista, "IDUser", "FullName", listUserSelect);
        }


        [HttpPost]
        public ActionResult Save(Incidence incidence)
        {
            IServiceIncidence _ServiceIncidence = new ServiceIncidence();
            try
            {

                if (ModelState.IsValid)
                {
                    Incidence oIncidence = _ServiceIncidence.Save(incidence);
                    TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Incidence", "Reported Incidence", Util.SweetAlertMessageType.success);
                }
                else
                {
                    ViewBag.IDUser = listUsers(incidence.IDUser);
                    //Lógica para cargar vista correspondiente
                    return View("Create", incidence);
                }
                
                return RedirectToAction("Create");

                
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
    }
}