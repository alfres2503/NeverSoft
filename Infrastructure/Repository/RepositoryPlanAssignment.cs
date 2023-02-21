using Infrastructure.Models;
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
                       .Include("AssignmentDetail")
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
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
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
                       .Include("AssignmentDetail")
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
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<AssignmentDetail> GetDetailsByAssignmentID(int id)
        {
            IEnumerable<AssignmentDetail> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.AssignmentDetail
                       .Where(r => r.IDAssignment == id)
                       .Include("PlanAssignment")
                       .Include("PlanAssignment.PaymentPlan")
                       .Include("PlanAssignment.Residence")
                       .Include("PlanAssignment.Residence.User")
                       .ToList<AssignmentDetail>();
                }
                return list;
            }
            catch (DbUpdateException dbEx)
            {
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
