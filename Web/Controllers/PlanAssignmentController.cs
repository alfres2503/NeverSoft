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
    public class PlanAssignmentController : Controller
    {
        // GET: PlanAssignment
        public ActionResult Index()
        {
            IEnumerable<PlanAssignment> list = null;
            try
            {
                IServicePlanAssignment _ServicePlanAssignment = new ServicePlanAssignment();
                list = _ServicePlanAssignment.GetPlanAssignments();
            } 
            catch(Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(list);
        }

        // GET: PlanAssignment/Details/5
        public ActionResult Details(int? id)
        {
            IServicePlanAssignment _ServicePlanAssignment = new ServicePlanAssignment();
            PlanAssignment oPlanAssignment = null;
            try
            {
                if(id == null)
                {
                    return RedirectToAction("Index");
                }
                oPlanAssignment = _ServicePlanAssignment.GetPlanAssignmentByID(Convert.ToInt32(id));
                if (oPlanAssignment == null)
                {
                    TempData["Message"] = "The requested PlanAssignment does not exist";
                    //Controlador
                    TempData["Redirect"] = "PlanAssignment";
                    //Acción
                    TempData["Redirect-Action"] = "Index";
                    return RedirectToAction("Default", "Error");
                }
                return View(oPlanAssignment);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                //Controlador
                TempData["Redirect"] = "PlanAssignment";
                //Acción
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }


    }
}