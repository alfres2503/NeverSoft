using Infrastructure.Models;
using Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
    }
}
