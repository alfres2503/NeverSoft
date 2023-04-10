using ApplicationCore.Services;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
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
            if(GetSessionUser() == null)
                return RedirectToAction("Index", "Reservation"); //Comentarios solo para no tener que estar logeando, hay que activarlos
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            ViewBag.IDUser = listUser(
                GetSessionUser().IDUser
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
            IEnumerable<User> lista = _ServiceUser.GetUsers()
            .Where(u => u.IDUser == idUser); /*Comentado solo para no tener que estar logeando, hay que activarlo*/

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

                SendMail(_ServiceReservation.GetReservationByID(id));

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

                SendMail(_ServiceReservation.GetReservationByID(id));

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
                     GetSessionUser().IDUser
                     );
                    ViewBag.IDArea = listAreas();
                    return View("Create", reservation);
                }

                Reservation oReservation = _ServiceReservation.Save(reservation);
                TempData["mensaje"] = Util.SweetAlertHelper.Mensaje("Reservation", "Reservation made successfully", Util.SweetAlertMessageType.success);
                return RedirectToAction("Create");

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
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

        private void SendMail(Reservation reservation)
        {
            IServiceUser _ServiceUser = new ServiceUser();
            IServiceArea _ServiceArea = new ServiceArea();
            User user = _ServiceUser.GetUserByID(reservation.IDUser);
            try
            {
                MailMessage message = new MailMessage();
                // neversoftinfo@gmail.com y0ry1wh1te rskktezrjfnqsaeo
                message.From = new MailAddress("neversoftinfo@gmail.com", "Neverland Administration");
                message.To.Add(user.Email);

                message.Subject = (reservation.Approved == true) ? "Reservation approved!" : "Reservation denied";

                message.Body = (reservation.Approved == true) ? $@"<h1>Reservation Approved</h1><p> Dear {user.FirstName}, <br><br> We are pleased to inform you that your reservation for the {_ServiceArea.GetAreaByID(reservation.IDArea).Name} area has been approved! You may now enjoy the amenities on the specified date and time. <br><br> Your reservation details are as follows:<br> Date: {reservation.Start.Date.ToShortDateString()}<br> Time: {reservation.Start.ToShortTimeString()} - {reservation.Finish.ToShortTimeString()}<br> <br> Please note that you are responsible for the conduct of your guests during your reservation, and that you must follow all area rules and guidelines to ensure the safety and enjoyment of all guests. <br><br> Thank you for choosing Neverland, and we hope you have a pleasant experience! <br><br> Best regards, <br> Neverland </p>" : $@"<h1>Reservation Denied</h1> <p> Dear {user.FirstName}, <br><br> We regret to inform you that your reservation for the {_ServiceArea.GetAreaByID(reservation.IDArea).Name} area has been denied. We apologize for any inconvenience this may cause. <br><br> Your reservation details are as follows:<br> Date: {reservation.Start.Date.ToShortDateString()}<br> Time: {reservation.Start.ToShortTimeString()} - {reservation.Finish.ToShortTimeString()}<br> <br> Please note that we reserve the right to deny any reservation that does not comply with our rules and guidelines. <br><br> Thank you for your understanding, and we hope that you will consider making a reservation with us in the future.<br><br> <br><br> Best regards, <br> Neverland </p>";
                message.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("neversoftinfo@gmail.com", "rskktezrjfnqsaeo");
                smtp.EnableSsl = true;

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at sending mail: " + ex.Message;
            }
        }
    }
}