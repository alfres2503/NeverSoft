using ApplicationCore.Services;
using Infrastructure.Models;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class PlanAssignmentController : Controller
    {
        // GET: PlanAssignment
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

        // GET: PlanAssignment/Details/5
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

        // GET: PlanAssignment/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlanAssignment/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PlanAssignment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlanAssignment/Edit/5
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
