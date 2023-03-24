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
    public class NewsController : Controller
    {
        // GET: News
        public ActionResult Index()
        {
            IEnumerable<News> lista = null;
            try
            {
                IServiceNews _ServiceNews = new ServiceNews();
                lista = _ServiceNews.GetNews();

                IServiceNewsCategory _ServiceNewsCategory = new ServiceNewsCategory();
                ViewBag.listNewsCategory = _ServiceNewsCategory.GetNewsCategory();

                //Esto tiene que ver con lo del login
                if (TempData.ContainsKey("mensaje"))
                {
                    ViewBag.NotificationMessage = TempData["mensaje"];
                }


                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        public ActionResult Maintenance()
        {
            IEnumerable<News> lista = null;
            try
            {
                IServiceNews _ServiceNews = new ServiceNews();
                lista = _ServiceNews.GetNews();


                return View(lista);
            }
            catch (Exception ex)
            {
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error at procesing data: " + ex.Message;

                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }
        public PartialViewResult NewsByCategory(int? id)
        {
            IEnumerable<News> list = null;
            IServiceNews _ServiceNews = new ServiceNews();
            if (id != null)
            {
                if (id == 0)
                {
                    list = _ServiceNews.GetNews();
                }
                else
                {
                    list = _ServiceNews.GetNewsByCategory((int)id);
                }
            }
            return PartialView("_PartialViewNews", list);
        }


        // GET: News/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: News/Create
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.IDCategory = listCategories();

            return View();
        }

        // POST: News/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //        return View();

        //}

        private MultiSelectList listCategories(ICollection<NewsCategory> newsCategory = null)
        {
            IServiceNewsCategory _ServiceNewsCategory = new ServiceNewsCategory();
            IEnumerable<NewsCategory> lista = _ServiceNewsCategory.GetNewsCategory();
            //Seleccionar categorias
            int[] listNewsCategorySelect = null;
            if (newsCategory != null)
            {

                listNewsCategorySelect = newsCategory.Select(c => c.IDCategory).ToArray();
            }

            return new MultiSelectList(lista, "IDCategory", "Description", listNewsCategorySelect);


        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            ServiceNews _ServiceNews = new ServiceNews();
            News news = null;

            try
            {
                // Si va null
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                news = _ServiceNews.GetNewsByID(Convert.ToInt32(id));
                if (news == null)
                {
                    TempData["Message"] = "The requested News does not exist";
                    TempData["Redirect"] = "News";
                    TempData["Redirect-Action"] = "Maintenance";
                    // Redireccion a la captura del Error
                    return RedirectToAction("Default", "Error");
                }
                ViewBag.IDCategory = listCategories();
                return View(news);
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

        // POST: News/Edit/5
        [HttpPost]
        public ActionResult Save(News news, HttpPostedFileBase File)
        {
            //Gestión de archivos
            MemoryStream target = new MemoryStream();
            //Servicio 
            IServiceNews _ServiceNews = new ServiceNews();
            try
            {
                //Insertar el archivo
                //if (news.Archive == null)
                //{
                if (File != null)
                {
                    //File.InputStream.CopyTo(target);
                    //news.Archive = target.ToArray();
                    //ModelState.Remove("Archive");
                    var archivoP = new byte[File.ContentLength];
                    File.InputStream.Read(archivoP, 0, File.ContentLength);
                    news.Archive = archivoP;
                }
                //}
                if (ModelState.IsValid)
                {
                    News oNewsF = _ServiceNews.Save(news);
                }
                else
                {
                    //Cargar la vista crear o actualizar
                    ViewBag.IDCategory = listCategories();

                    //Lógica para cargar vista correspondiente
                    return View("Create", news);
                }

                return RedirectToAction("Maintenance");
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

        // GET: News/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: News/Delete/5
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

        public ActionResult DownloadPDF(int? id)
        {
            IServiceNews _Service = new ServiceNews();
            News oNews = _Service.GetNewsByID(Convert.ToInt32(id));
            return File(oNews.Archive, "application/pdf", "NeverLand-Information.pdf");
        }

        //public ActionResult RemovePDF(int? id)
        //{
        //    IServiceNews _Service = new ServiceNews();
        //    News oNews = _Service.GetNewsByID(Convert.ToInt32(id));
        //    oNews.Archive = null;
        //    return File(oNews.Archive,);
        //}
    }
}
