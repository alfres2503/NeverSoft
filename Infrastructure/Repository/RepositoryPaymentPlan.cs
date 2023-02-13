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
    public class RepositoryPaymentPlan : IRepositoryPaymentPlan
    {
        public PaymentPlan GetPaymentPlanByID(int id)
        {
            PaymentPlan oPaymentPlan = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oPaymentPlan = ctx.PaymentPlan
                        .Where(r => r.IDPlan == id).
                        Include("PaymentItem")
                        .FirstOrDefault();
                }
                return oPaymentPlan;
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

        public IEnumerable<PaymentPlan> GetPaymentPlans()
        {
            try
            {
                IEnumerable<PaymentPlan> list = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    //list = ctx.Residence.Include("User").ToList<Residence>();
                    list = ctx.PaymentPlan.Include("PaymentItem").ToList<PaymentPlan>();
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
