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
    public class PaymentItemController : Controller
    {
        // GET: PaymentItem
        public ActionResult Index()
        {
            IEnumerable<PaymentItem> lista = null;
            try
            {
                IServicePaymentItem _ServicePaymentItem = new ServicePaymentItem();
                lista = _ServicePaymentItem.GetPaymentItem();


                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        // GET: PaymentItem/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PaymentItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentItem/Create
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

        // GET: PaymentItem/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PaymentItem/Edit/5
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

        // GET: PaymentItem/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PaymentItem/Delete/5
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
