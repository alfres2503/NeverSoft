using ApplicationCore.Services;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class StatementAccountController : Controller
    {
        // GET: StatementAccount
        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult Index()
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
        

        public ActionResult Details(int? id)
        {
            IServiceResidence _ServiceResidence = new ServiceResidence();
            Residence oResidence = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                oResidence = _ServiceResidence.GetResidenceByID(Convert.ToInt32(id));
                if (oResidence == null)
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

        public ActionResult PayDebts()
        {
            IServiceResidence _ServiceResidence = new ServiceResidence();
            
            Residence oResidence;
            try
            {
                oResidence = _ServiceResidence.GetResidenceByUser(GetSessionUser().IDUser);
                if (oResidence == null)
                {
                    TempData["Message"] = "The requested data had a problem";
                    //Controlador
                    TempData["Redirect"] = "Index";
                    //Acción
                    TempData["Redirect-Action"] = "Index";
                    return RedirectToAction("Default", "Error");
                }
                ViewBag.PendingDebts = listDebts(oResidence.IDResidence); 
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

        public ActionResult SetDebtsAsPaid(string[] selectedDebts) 
        {
            IServicePlanAssignment _ServicePlanAssignment = new ServicePlanAssignment();
            IServiceResidence _ServiceResidence = new ServiceResidence();
            try
            {

                _ServicePlanAssignment.SetDebtsAsPaid(selectedDebts);
                

                ViewBag.PendingDebts = listDebts(_ServiceResidence.GetResidenceByUser(GetSessionUser().IDUser).IDResidence);
                return RedirectToAction("PayDebts");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                TempData["Redirect"] = "PaymentPlan";
                TempData["Redirect-Action"] = "Maintenance";
                // Redireccion a la captura del Error
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

        private MultiSelectList listDebts(int idResidence)
        {
            IServicePlanAssignment _ServicePlanAssignment = new ServicePlanAssignment();
            IEnumerable<PlanAssignment> list = _ServicePlanAssignment.GetDebtsByResidence(idResidence);

            return new MultiSelectList(list, "IDAssignment", "Description");
        }
    }
}