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
    public class PaymentPlanController : Controller
    {
        // GET: PaymentPlan
        public ActionResult Index()
        {
            IEnumerable<PaymentPlan> list = null;
            try
            {
                IServicePaymentPlan _ServicePaymentPlan = new ServicePaymentPlan();
                list = _ServicePaymentPlan.GetPaymentPlans();
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                return RedirectToAction("Default", "Error");
            }
            return View(list);
        }

        // GET: PaymentPlan/Details/5
        public ActionResult Details(int? id)
        {
            IServicePaymentPlan _ServicePaymentPlan = new ServicePaymentPlan();
            PaymentPlan oPaymentPlan = null;
            try
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }
                oPaymentPlan = _ServicePaymentPlan.GetPaymentPlanByID(Convert.ToInt32(id));
                if (oPaymentPlan == null)
                {
                    TempData["Message"] = "The requested PaymentPlan does not exist";
                    //Controlador
                    TempData["Redirect"] = "PaymentPlan";
                    //Acción
                    TempData["Redirect-Action"] = "Index";
                    return RedirectToAction("Default", "Error");
                }
                return View(oPaymentPlan);
            }
            catch (Exception ex)
            {

                TempData["Message"] = "Error at procesing data: " + ex.Message;
                //Controlador
                TempData["Redirect"] = "PaymentPlan";
                //Acción
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: PaymentPlan/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentPlan/Create
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

        // GET: PaymentPlan/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaymentPlan/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: PaymentPlan/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaymentPlan/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
