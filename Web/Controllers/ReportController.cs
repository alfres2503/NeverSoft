using ApplicationCore.Services;
using Infrastructure.Models;
using Infrastructure.Utils;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Web.Security;

namespace Web.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View("Debts");
        }

        [CustomAuthorize((int)UserRoles.Administrator)]
        public ActionResult Debts(int? residence, int? month)
        {

            try
            {
                IEnumerable<PlanAssignment> lista = null;
                using (MyContext ctx = new MyContext())
                {
                    // Obtener la lista de residencias disponibles
                    var residences = ctx.Residence.ToList();
                    // Crear la lista de opciones para el DropDownList de residencias
                    List<SelectListItem> residenceOptions = residences.Select(r => new SelectListItem
                    {
                        Text = r.IDResidence.ToString(),
                        Value = r.IDResidence.ToString(),
                    }).ToList();



                    // Pasar la lista de residencias a la vista
                    ViewBag.residence = residenceOptions;

                    List<SelectListItem> months = new List<SelectListItem>();

                    for (int i = 1; i <= 12; i++)
                    {
                        months.Add(new SelectListItem { Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i), Value = i.ToString() });
                    }

                    ViewBag.month = months;
                    IServicePlanAssignment _ServiceLibro = new ServicePlanAssignment();
                    lista = _ServiceLibro.GetPlanAssignments().Where(p => (!residence.HasValue || p.IDResidence == residence.Value)
                && (!month.HasValue || p.AssignmentDate.Month == month.Value)
                && !p.PayedStatus);
                    return View(lista);
                }

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }
        }

        [HttpPost]
        public ActionResult CreatePdfDebts(int? residence, int? month)
        {

            IEnumerable<PlanAssignment> lista = null;
            try
            {
                // Extraer informacion
                IServicePlanAssignment _ServiceLibro = new ServicePlanAssignment();
                lista = _ServiceLibro.GetPlanAssignments()
           .Where(p => (!residence.HasValue || p.IDResidence == residence.Value)
           && (!month.HasValue || p.AssignmentDate.Month == month.Value)
               && !p.PayedStatus);



                // Crear stream para almacenar en memoria el reporte 
                MemoryStream ms = new MemoryStream();
                //Inicializar writer
                PdfWriter writer = new PdfWriter(ms);

                //Inicializar document
                PdfDocument pdfDoc = new PdfDocument(writer);
                Document doc = new Document(pdfDoc);



                //Título
                Paragraph header = new Paragraph("Debts - " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Value))
                                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                                    .SetBold()
                                    .SetFontSize(14)
                                    .SetFontColor(ColorConstants.BLACK);
                doc.Add(header);


                // Crear tabla con 5 columnas 
                Table table = new Table(5, true);
                //Encabezados de la tabla
                table.AddHeaderCell(new Cell().Add(new Paragraph("Residence")).SetBackgroundColor(new DeviceRgb(0xFF, 0xA6, 0x2B)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("User")).SetBackgroundColor(new DeviceRgb(0xFF, 0xA6, 0x2B)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Assignment Date")).SetBackgroundColor(new DeviceRgb(0xFF, 0xA6, 0x2B)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Payed Status")).SetBackgroundColor(new DeviceRgb(0xFF, 0xA6, 0x2B)));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Amount")).SetBackgroundColor(new DeviceRgb(0xFF, 0xA6, 0x2B)));



                decimal totalAmount = 0;
                foreach (var item in lista)
                {

                    // Agregar datos a las celdas
                    table.AddCell(new Paragraph(item.IDResidence.ToString()));
                    table.AddCell(new Paragraph(item.Residence.User.FullName.ToString()));
                    table.AddCell(new Paragraph(item.AssignmentDate.ToString("dd/MM/yyyy")));
                    table.AddCell(new Paragraph(item.PayedStatus ? "Paid" : "Not Paid"));
                    table.AddCell(new Paragraph(String.Format("${0}", item.Amount)));

                    // Actualizar la suma total
                    totalAmount += item.Amount;

                }
                table.AddCell(new Cell().Add(new Paragraph("Total")).SetBold().SetBackgroundColor(new DeviceRgb(0xFF, 0xA6, 0x2B)));
                table.AddCell(new Cell().Add(new Paragraph("")).SetBold().SetBackgroundColor(new DeviceRgb(0xFF, 0xA6, 0x2B)));
                table.AddCell(new Cell().Add(new Paragraph("")).SetBold().SetBackgroundColor(new DeviceRgb(0xFF, 0xA6, 0x2B)));
                table.AddCell(new Cell().Add(new Paragraph("")).SetBold().SetBackgroundColor(new DeviceRgb(0xFF, 0xA6, 0x2B)));
                table.AddCell(new Cell().Add(new Paragraph(String.Format("${0}", totalAmount))).SetBackgroundColor(new DeviceRgb(0xBE, 0x4D, 0x25)));


                doc.Add(table);

                // Colocar número de páginas
                int numberofPages = pdfDoc.GetNumberOfPages();
                for (int i = 1; i <= numberofPages; i++)
                {
                    //Texto alineado a un punto específico
                    doc.ShowTextAligned(
                        new Paragraph(
                            String.Format("page {0} de {1}", i, numberofPages)
                            ),
                        559, 826, i, TextAlignment.RIGHT, VerticalAlignment.TOP, 0
                        );
                }


                //Terminar document
                doc.Close();
                // Retorna un File
                return File(ms.ToArray(), "application/pdf", "Debts Report.pdf");

            }
            catch (Exception ex)
            {
                // Salvar el error en un archivo 
                Log.Error(ex, MethodBase.GetCurrentMethod());
                TempData["Message"] = "Error al procesar los datos! " + ex.Message;
                TempData.Keep();
                // Redireccion a la captura del Error
                return RedirectToAction("Default", "Error");
            }

        }
    }
}
