using ApplicationCore.Services;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class ResidenceController : Controller
    {
        // GET: Residence
        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult Index()
        {
            IEnumerable<Residence> list = null;
            try
            {
                IServiceResidence _ServiceResidence = new ServiceResidence();
                list = _ServiceResidence.GetResidences();
            } catch(Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(list);
        }


        // GET: Residence/Details/5
        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult Details(int? id)
        {
            IServiceResidence _ServiceResidence = new ServiceResidence();
            Residence oResidence = null;
            try
            {
                if(id == null)
                {
                    return RedirectToAction("Index");
                }
                oResidence = _ServiceResidence.GetResidenceByID(Convert.ToInt32(id));
                if(oResidence == null)
                {
                    TempData["Message"] = "The requested residence does not exist";
                    //Controlador
                    TempData["Redirect"] = "Residence";
                    //Acción
                    TempData["Redirect-Action"] = "Index";
                    return RedirectToAction("Default", "Error");
                }
                return View(oResidence);
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
        
        public ActionResult Create()
        {
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            ViewBag.Plans = listPlans();
            ViewBag.IDUser = listUsers();
            ViewBag.NewResidenceID = new ServiceResidence().GetResidences().Last().IDResidence + 1;
            return View();
        }

        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult Maintenance()
        {
            IEnumerable<Residence> list = null;
            try
            {
                IServiceResidence _ServiceResidence = new ServiceResidence();
                list = _ServiceResidence.GetResidences();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(list);
        }

        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult Edit(int? id)
        {
            IServiceResidence _ServiceResidence = new ServiceResidence();
            Residence residence = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Maintenance");
                }

                residence = _ServiceResidence.GetResidenceByID(Convert.ToInt32(id));
                if (residence == null)
                {
                    TempData["Message"] = "The requested User does not exist";
                    TempData["Redirect"] = "User";
                    TempData["Redirect-Action"] = "Maintenance";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                ViewBag.IDUser = listUsers(residence.IDUser);
                return View(residence);
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

        public ActionResult SaveCreate(Residence residence, int idPlan)
        {
            IServiceResidence _ServiceResidence = new ServiceResidence();
            IServicePlanAssignment _ServicePlanAssignment = new ServicePlanAssignment();
            IServicePaymentPlan _ServicePaymentPlan = new ServicePaymentPlan();
            try
            {
                if (ModelState.IsValid)
                {
                    Residence oResidence = _ServiceResidence.Save(residence);
                    PaymentPlan PP = _ServicePaymentPlan.GetPaymentPlanByID(idPlan);
                    _ServicePlanAssignment.Save(new PlanAssignment
                    {
                        IDPlan = idPlan,
                        IDResidence = residence.IDResidence,
                        AssignmentDate = DateTime.Now,
                        PayedStatus = false,
                        Amount = PP.Total
                    });
                }
                else
                {
                    ViewBag.Plans = listPlans();
                    ViewBag.IDUser = listUsers();
                    ViewBag.NewResidenceID = new ServiceResidence().GetResidences().Last().IDResidence + 1;
                    return View("Create", residence);
                }

                return RedirectToAction("Maintenance");

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

        public ActionResult SaveEdit(Residence residence)
        {
            IServiceResidence _ServiceResidence = new ServiceResidence();
            try
            {
                if (ModelState.IsValid)
                {
                    Residence oResidence = _ServiceResidence.Save(residence);
                }
                else
                {
                    ViewBag.IDUser = listUsers();
                    ViewBag.NewResidenceID = new ServiceResidence().GetResidences().Last().IDResidence + 1;
                    return View("Edit", residence);
                }

                return RedirectToAction("Maintenance");

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

        private SelectList listUsers(long idUser = 0)
        {
            IServiceUser _ServiceUser = new ServiceUser();
            IEnumerable<User> lista = _ServiceUser.GetUsers()
                .Where(u => u.IDRole == 2);

            return new SelectList(lista, "IDUser", "FullName", idUser);
        }

        private SelectList listPlans()
        {
            IServicePaymentPlan _ServicePlans = new ServicePaymentPlan();
            IEnumerable<PaymentPlan> lista = _ServicePlans.GetPaymentPlans();


            return new SelectList(lista, "IDPlan", "NameAndPrice");
        }
    }
}