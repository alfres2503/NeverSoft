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
            return View(list);
        }

        public ActionResult UserReservations(long? idUser)
        {
            IServiceReservation _ServiceReservation = new ServiceReservation();
            IEnumerable<Reservation> list = null;
            try
            {
                if (idUser == null)
                {
                    return RedirectToAction("Index");
                }
                list = _ServiceReservation.GetReservationsByUser((long)Convert.ToDouble(idUser));
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
            return View();
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
    }
}