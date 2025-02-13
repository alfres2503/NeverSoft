﻿using ApplicationCore.Services;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Web.Security;
using Web.Utils;

namespace Web.Controllers
{
    public class PaymentPlanController : Controller
    {
        // GET: PaymentPlan
        [CustomAuthorize((int)UserRoles.Administrator)]
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
        [CustomAuthorize((int)UserRoles.Administrator)]
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
                //Contrsolador
                TempData["Redirect"] = "PaymentPlan";
                //Acción
                TempData["Redirect-Action"] = "Index";
                return RedirectToAction("Default", "Error");
            }
        }

        //Mantenimiento
        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult Maintenance()
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
        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult Edit(int? id)
        {
            ServicePaymentPlan _ServicePaymentPlan = new ServicePaymentPlan();
            PaymentPlan paymentPlan = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                paymentPlan = _ServicePaymentPlan.GetPaymentPlanByID(Convert.ToInt32(id));
                if (paymentPlan == null)
                {
                    TempData["Message"] = "The requested PaymentPlan does not exist";
                    TempData["Redirect"] = "PaymentPlan";
                    TempData["Redirect-Action"] = "Index";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }

                ViewBag.IDItem = listPaymentItems(paymentPlan.PaymentItem);
                return View(paymentPlan);
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                TempData["Redirect"] = "PaymentPlan";
                TempData["Redirect-Action"] = "Index";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }



       

        // GET: PaymentPlan/Create
        [HttpGet]
        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult Create()
        {
          
            
            
            ViewBag.IDItem = listPaymentItems();

            return View();
        }

        private MultiSelectList listPaymentItems(ICollection<PaymentItem> paymentItems = null)
        {
            IServicePaymentItem _ServicePaymentItem = new ServicePaymentItem();
            IEnumerable<PaymentItem> lista = _ServicePaymentItem.GetPaymentItem();
            //Seleccionar categorias
            int[] listPaymentItemSelect = null;
            if (paymentItems != null)
            {
                listPaymentItemSelect = paymentItems.Select(c => c.IDItem).ToArray();
            }

            return new MultiSelectList(lista, "IDItem", "DescriptionAndPrice", listPaymentItemSelect);
        }
        

        // POST: PaymentPlan/Edit/5
        [HttpPost]
        public ActionResult Save(PaymentPlan paymentPlan, string[] selectedPaymentItems)
        {
            //Gestión de archivos
            MemoryStream target = new MemoryStream();
            
            IServicePaymentPlan _ServicePaymentPlan = new ServicePaymentPlan();
            try
            {
                
                if (ModelState.IsValid)
                {
                    PaymentPlan oPaymentPlanI = _ServicePaymentPlan.Save(paymentPlan, selectedPaymentItems);
                }
                else
                {
                    

                   
                    ViewBag.IDItem = listPaymentItems(paymentPlan.PaymentItem);
                    //Lógica para cargar vista correspondiente
                    return View("Create", paymentPlan);
                }

                return RedirectToAction("Maintenance");
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

      
        
       
    }
}
