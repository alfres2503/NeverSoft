using ApplicationCore.Services;
using Infrastructure.Models;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.Security;
using Web.ViewModel;

namespace Web.Controllers
{
    public class PlanAssignmentController : Controller
    {
        // GET: PlanAssignment
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
            if (TempData.ContainsKey("mensaje"))
            {
                ViewBag.NotificationMessage = TempData["mensaje"];
            }
            return View(list);
        }

        // GET: PlanAssignment/Details/5
        [CustomAuthorize((int)UserRoles.Administrator)]
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

        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult AddMonthlyAssignment(int? idPlanAssignment)
        {
            IServicePlanAssignment _ServicePA = new ServicePlanAssignment();
            PlanAssignment PA = null;

            try
            {
                // Si va null
                if (idPlanAssignment == null)
                {
                    return RedirectToAction("Index");
                }

                PA = _ServicePA.GetPlanAssignmentByID(Convert.ToInt32(idPlanAssignment));
                if (PA == null)
                {
                    TempData["Message"] = "The requested Plan Assignment does not exist";
                    TempData["Redirect"] = "PlanAssignment";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                ViewBag.IDPlan = listPlans(PA.IDPlan);
                ViewBag.IDResidence = listResidences(PA.IDResidence);
                ViewBag.DefaultAssignmentDate = DateTime.Now;
                return View();
            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;
                TempData["Redirect"] = "News";
                TempData["Redirect-Action"] = "Maintenance";
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }

        }

        private SelectList listPlans(int idPlan)
        {
            IServicePaymentPlan _ServicePlans = new ServicePaymentPlan();
            IEnumerable<PaymentPlan> lista = _ServicePlans.GetPaymentPlans();


            return new SelectList(lista, "IDPlan", "NameAndPrice", idPlan);
        }

        private SelectList listResidences(int idResidence)
        {
            IServiceResidence _ServiceResidence = new ServiceResidence();
            IEnumerable<Residence> lista = _ServiceResidence.GetResidences()
                .Where(r => r.IDResidence == idResidence);


            return new SelectList(lista, "IDResidence", "IDResidence", idResidence);
        }

        // POST: PlanAssignment/Edit/5
        [HttpPost]
        public ActionResult Save(PlanAssignment planAssignment)
        {
            IServicePlanAssignment _ServicePlanAssignment = new ServicePlanAssignment();
            try
            {
                //if that checks if theres another plan assignment with the same month and year
                if (_ServicePlanAssignment.GetPlanAssignmentByMonthAndYear(planAssignment.AssignmentDate.Month, planAssignment.AssignmentDate.Year, planAssignment.IDResidence) != null)
                {
                    ViewBag.NotificationMessage = Util.SweetAlertHelper.Mensaje("Error", "Month already charged!", Util.SweetAlertMessageType.error);
                    ViewBag.IDPlan = listPlans(planAssignment.IDPlan);
                    ViewBag.IDResidence = listResidences(planAssignment.IDResidence);
                    ViewBag.DefaultAssignmentDate = DateTime.Now;
                    return View("AddMonthlyAssignment", planAssignment);
                }

                if (ModelState.IsValid)
                {
                    PlanAssignment oPlanAssigmentI = _ServicePlanAssignment.Save(planAssignment);
                }
                else
                {
                    
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

        public ActionResult Incomesgraphic()
        {

            IServicePlanAssignment _ServicePlanAssignment = new ServicePlanAssignment();
            ViewModelGrafico grafico = new ViewModelGrafico();
            _ServicePlanAssignment.GetMonthlyIncomesOfTheCurrentYear(out string etiquetas, out string valores);
            grafico.Etiquetas = etiquetas;
            grafico.Valores = valores;
            int cantidadValores = valores.Split(',').Length;
            grafico.Colores = string.Join(",", grafico.GenerateColors(cantidadValores));
            grafico.titulo = "Total";
            grafico.tituloEtiquetas = "Monthly Incomes of the Current Year";
            //Tipos de gráficos: bar, bubble, doughnut, pie, line, polarArea
            grafico.tipo = "bar";
            ViewBag.grafico = grafico;
            return View();
        }

    }
}
