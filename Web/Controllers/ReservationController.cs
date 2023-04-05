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
    public class ReservationController : Controller
    {
        // GET: Reservation
        public ActionResult Index()
        {
            IEnumerable<Reservation> list = null;
            try
            {
                IServiceReservation _ServiceReservation = new ServiceReservation();
                list = _ServiceReservation.GetReservations();
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

        public ActionResult UserReservations()
        {
            IServiceReservation _ServiceReservation = new ServiceReservation();
            IEnumerable<Reservation> list = null;
            try
            {
                if (GetSessionUser() == null)
                {
                    return RedirectToAction("Index");
                }
                list = _ServiceReservation.GetReservationsByUser((long)Convert.ToDouble(GetSessionUser().IDUser));
                if (list == null)
                {
                    TempData["Message"] = "The requested reservations does not exist";
                    //Controlador
                    TempData["Redirect"] = "Reservation";
                    //Acción
                    TempData["Redirect-Action"] = "Index";
                    return RedirectToAction("Default", "Error");
                }
                return View(list);
            }
            catch (Exception ex)
            {

                TempData["Message"] = "Error at procesing data: " + ex.Message;
                //Controlador
                TempData["Redirect"] = "Residence";
                //Acción
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        [HttpGet]
        public ActionResult Create()
        {
            //if(GetSessionUser() == null)
            //    return RedirectToAction("Index", "Reservation"); Comentarios solo para no tener que estar logeando, hay que activarlos

            ViewBag.IDUser = listUser(
                //GetSessionUser().IDUser
                );
            ViewBag.IDArea = listAreas();
            return View();
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

        private SelectList listUser(long idUser = 0)
        {
            IServiceUser _ServiceUser = new ServiceUser();
            IEnumerable<User> lista = _ServiceUser.GetUsers();
            //.Where(u => u.IDUser == idUser); Comentado solo para no tener que estar logeando, hay que activarlo

            return new SelectList(lista, "IDUser", "FullName", idUser);
        }

        private SelectList listAreas(int idArea = 0)
        {
            IServiceArea _ServiceArea = new ServiceArea();
            IEnumerable<Area> lista = _ServiceArea.GetAreas();
            //.Where(u => u.IDUser == idUser);

            return new SelectList(lista, "IDArea", "Name", idArea);
        }


        public ActionResult Details(int? id)
        {
            IServiceReservation _ServiceReservation = new ServiceReservation();
            Reservation oReservation = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                oReservation = _ServiceReservation.GetReservationByID(Convert.ToInt32(id));
                if (oReservation == null)
                {
                    TempData["Message"] = "The requested reservation does not exist";
                    //Controlador
                    TempData["Redirect"] = "Reservation";
                    //Acción
                    TempData["Redirect-Action"] = "Index";
                    return RedirectToAction("Default", "Error");
                }
                return View(oReservation);
            }
            catch (Exception ex)
            {

                TempData["Message"] = "Error at procesing data: " + ex.Message;
                //Controlador
                TempData["Redirect"] = "Residence";
                //Acción
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult Approve(int id)
        {
            try
            {
                IServiceReservation _ServiceReservation = new ServiceReservation();
                _ServiceReservation.Approve(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                TempData["Redirect"] = "Residence";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult Deny(int id)
        {
            try
            {
                IServiceReservation _ServiceReservation = new ServiceReservation();
                _ServiceReservation.Deny(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                TempData["Redirect"] = "Residence";
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }
        [HttpPost]
        public ActionResult Save(Reservation reservation)
        {
            IServiceReservation _ServiceReservation = new ServiceReservation();
            try
            {
                string errorMessage = validateReservation(reservation);
                if (!string.IsNullOrEmpty(errorMessage))
                { 
                    ModelState.AddModelError("", errorMessage);
                    ViewBag.IDUser = listUser(
                     //GetSessionUser().IDUser
                     );
                    ViewBag.IDArea = listAreas();
                    return View("Create", reservation);
                }

                Reservation oReservation = _ServiceReservation.Save(reservation);
                return RedirectToAction("Index");

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

        private string validateReservation(Reservation reservation)
        {
            Area oArea = new ServiceArea().GetAreaByID(reservation.IDArea);
            if (reservation.Start.Date != reservation.Finish.Date)
                return "Reservations must be on the same day";
            else if (reservation.Start.TimeOfDay < oArea.OpeningHour || reservation.Start.TimeOfDay > oArea.ClosureHour)
                return $"Area opens at {oArea.OpeningHour.ToString(@"hh\:mm")}";
            else if (reservation.Finish.TimeOfDay > oArea.ClosureHour || reservation.Finish.TimeOfDay < oArea.OpeningHour)
                return $"Area closes at {oArea.ClosureHour.ToString(@"hh\:mm")}";
            foreach (Reservation item in new ServiceReservation().GetReservationsByDate(reservation.Start.DayOfYear, reservation.IDArea))
            {
                if ((reservation.Start.TimeOfDay == item.Start.TimeOfDay)
                    || (reservation.Finish.TimeOfDay == item.Finish.TimeOfDay)
                    || (reservation.Start.TimeOfDay < item.Start.TimeOfDay && reservation.Finish.TimeOfDay >= item.Start.TimeOfDay)
                    || (reservation.Start.TimeOfDay > item.Start.TimeOfDay && reservation.Start.TimeOfDay < item.Finish.TimeOfDay)
                    || (reservation.Start.TimeOfDay > item.Start.TimeOfDay && reservation.Finish.TimeOfDay < item.Finish.TimeOfDay))
                    return $"There is a reservation already from {item.Start.TimeOfDay.ToString(@"hh\:mm")} to {item.Finish.TimeOfDay.ToString(@"hh\:mm")}";
            }
            return "";
        }
    }
}