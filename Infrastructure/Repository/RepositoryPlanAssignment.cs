using Infrastructure.Models;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RepositoryPlanAssignment : IRepositoryPlanAssignment
    {
        public IEnumerable<PlanAssignment> GetPlanAssignments()
        {
            try
            {
                IEnumerable<PlanAssignment> list = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.PlanAssignment
                       .Include("PaymentPlan")
                       .Include("PaymentPlan.PaymentItem")
                       .Include("Residence")
                       .Include("Residence.User")
                       .ToList<PlanAssignment>();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public PlanAssignment GetPlanAssignmentByID(int id)
        {
            PlanAssignment oPlanAssignment = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oPlanAssignment = ctx.PlanAssignment
                       .Where(r => r.IDAssignment == id)
                       .Include("PaymentPlan")
                       .Include("PaymentPlan.PaymentItem")
                       .Include("Residence")
                       .Include("Residence.User")
                       .FirstOrDefault();
                }
                return oPlanAssignment;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public PlanAssignment Save(PlanAssignment planAssignment)
        {
            int retorno = 0;
            PlanAssignment oPlanAssignment = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oPlanAssignment = GetPlanAssignmentByID((int)planAssignment.IDAssignment);


                    if (oPlanAssignment == null)
                    {

                        ctx.PlanAssignment.Add(planAssignment);
                        retorno = ctx.SaveChanges();

                    }
                    else
                    {

                        ctx.PlanAssignment.Add(planAssignment);
                        ctx.Entry(planAssignment).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();
                    }
                }

                if (retorno >= 0)
                    oPlanAssignment = GetPlanAssignmentByID((int)planAssignment.IDAssignment);

                return oPlanAssignment;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public PlanAssignment GetPlanAssignmentByMonthAndYear(int month, int year, int IdResidence)
        {
            PlanAssignment oPlanAssignment = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oPlanAssignment = ctx.PlanAssignment
                       .Where(r => r.AssignmentDate.Month == month && r.AssignmentDate.Year == year && r.IDResidence == IdResidence)
                       .Include("PaymentPlan")
                       .Include("PaymentPlan.PaymentItem")
                       .Include("Residence")
                       .Include("Residence.User")
                       .FirstOrDefault();
                }
                return oPlanAssignment;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public IEnumerable<PlanAssignment> GetDebtsByResidence(int idResidence)
        {
            try
            {
                IEnumerable<PlanAssignment> list = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.PlanAssignment
                       .Where(r => r.IDResidence == idResidence && r.PayedStatus == false)
                       .Include("PaymentPlan")
                       .Include("PaymentPlan.PaymentItem")
                       .Include("Residence")
                       .Include("Residence.User")
                       .ToList<PlanAssignment>();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public void SetDebtsAsPaid(string[] selectedDebts)
        {
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    foreach (string id in selectedDebts)
                        ctx.Database.ExecuteSqlCommand("UPDATE PlanAssignment SET PayedStatus = 1 WHERE IDAssignment = " + id);
                    
                }
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw;
            }
        }

        public void GetMonthlyIncomesOfTheCurrentYear(out string etiquetas, out string valores)
        {
            String varEtiquetas = "";
            String varValores = "";
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //Obtener ingresos mensuales del año actual

                    int year = DateTime.Now.Year;
                    var resultado = ctx.PlanAssignment
                    .Where(x => x.AssignmentDate.Year == year && x.PayedStatus == true)
                    .GroupBy(x => x.AssignmentDate.Month)
                    .Select(o => new { Total = o.Sum(x => x.Amount), Month = o.Key })
                    .ToList() // traer los datos a la memoria
                    .Select(o => new { Total = o.Total, Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(o.Month) });

                    //Crear etiquetas y valores
                    foreach (var item in resultado)
                    {
                        varEtiquetas += item.Month + ",";
                        varValores += item.Total.ToString() + ",";
                    }

                }
                //Ultima coma
                varEtiquetas = varEtiquetas.Substring(0, varEtiquetas.Length - 1); // ultima coma
                varValores = varValores.Substring(0, varValores.Length - 1);
                //Asignar valores de salida
                etiquetas = varEtiquetas;
                valores = varValores;
            }
            catch (DbUpdateException dbEx)
            {
                string mensaje = "";
                Log.Error(dbEx, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
            catch (Exception ex)
            {
                string mensaje = "";
                Log.Error(ex, System.Reflection.MethodBase.GetCurrentMethod(), ref mensaje);
                throw new Exception(mensaje);
            }
        }
    }
}
