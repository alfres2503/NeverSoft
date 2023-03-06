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

        public PaymentPlan Save(PaymentPlan paymentPlan)
        {
            int retorno = 0;
            PaymentPlan oPaymentP = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oPaymentP = this.GetPaymentPlanByID((int)paymentPlan.IDPlan);

                if (oPaymentP == null)
                {
                    //Insertar 
                    ctx.PaymentPlan.Add(paymentPlan);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                    //retorna número de filas afectadas
                }
                else
                {
                    //Actualizar 
                    ctx.PaymentPlan.Add(paymentPlan);
                    ctx.Entry(paymentPlan).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();
                }
            }

            if (retorno >= 0)
                oPaymentP = this.GetPaymentPlanByID((int)paymentPlan.IDPlan);

            return oPaymentP;
        }


    }
}
