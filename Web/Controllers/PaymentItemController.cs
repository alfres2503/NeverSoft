using ApplicationCore.Services;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
                TempData["Message"] = "Error at procesing data:  " + ex.Message;

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
        [HttpGet]
        public ActionResult Create(FormCollection collection)
        {
            
                return View();
            
        }

        // GET: PaymentItem/Edit/5
        public ActionResult Edit(int? id)
        {
            ServicePaymentItem _ServicePaymentItem = new ServicePaymentItem();
            PaymentItem paymentItem = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                paymentItem = _ServicePaymentItem.GetPaymentItemByID(Convert.ToInt32(id));
                if (paymentItem == null)
                {
                    TempData["Message"] = "The requested PaymentItem does not exist";
                    TempData["Redirect"] = "PaymentItem";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                
                return View(paymentItem);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                TempData["Redirect"] = "PaymentItem";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        // POST: PaymentItem/Edit/5
        [HttpPost]
        public ActionResult Save(PaymentItem paymentItem)
        {
            
            //Servicio Libro
            IServicePaymentItem _ServicePaymentItem = new ServicePaymentItem();
            try
            {

                if (ModelState.IsValid)
                {
                    PaymentItem oPaymentItemI = _ServicePaymentItem.Save(paymentItem);
                }
                else
                {
                
                    //Lógica para cargar vista correspondiente
                    return View("Create", paymentItem);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                TempData["Redirect"] = "PaymentItem";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

     
        
    }
}
