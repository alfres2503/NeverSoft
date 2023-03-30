using ApplicationCore.Services;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Utils;

namespace Web.Controllers
{
    public class StatementAccountController : Controller
    {
        // GET: StatementAccount
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

        [HttpPost]
        public ActionResult Save(PlanAssignment planAssignment)
        {
            IServicePlanAssignment _ServicePlanAssignment = new ServicePlanAssignment();
            try
            {
                
                if (ModelState.IsValid)
                {
                    PlanAssignment oPlanAssigmentI = _ServicePlanAssignment.Save(planAssignment);
                }
                else
                {
                   //Hay que colocar la vista correcta
                    return View("Create", planAssignment);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                TempData["Redirect"] = "StatementAccount";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
    }
}