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

        public IEnumerable<PaymentPlan> GetPaymentPlans()
        {
            try
            {
                IEnumerable<PaymentPlan> list = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.PaymentPlan.Include("PaymentItem").ToList<PaymentPlan>();
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

        public PaymentPlan Save(PaymentPlan paymentPlan, string[] selectedPaymentItems)
        {
            int retorno = 0;
            PaymentPlan oPaymentPlan = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oPaymentPlan = GetPaymentPlanByID((int)paymentPlan.IDPlan);
                    IRepositoryPaymentItem _RepositoryPaymentItem = new RepositoryPaymentItem();

                    if (oPaymentPlan == null)
                    {


                        if (selectedPaymentItems != null)
                        {

                            paymentPlan.PaymentItem = new List<PaymentItem>();
                            foreach (var paymentI in selectedPaymentItems)
                            {
                                var paymentItemToAdd = _RepositoryPaymentItem.GetPaymentItemByID(int.Parse(paymentI));
                                ctx.PaymentItem.Attach(paymentItemToAdd);
                                paymentPlan.PaymentItem.Add(paymentItemToAdd);


                            }
                        }

                        ctx.PaymentPlan.Add(paymentPlan);

                        retorno = ctx.SaveChanges();

                    }
                    else
                    {

                        ctx.PaymentPlan.Add(paymentPlan);
                        ctx.Entry(paymentPlan).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();


                        var selectedPaymentItemID = new HashSet<string>(selectedPaymentItems);
                        if (selectedPaymentItems != null)
                        {
                            ctx.Entry(paymentPlan).Collection(p => p.PaymentItem).Load();
                            var newPaymenItemForPaymentPlan = ctx.PaymentItem
                             .Where(x => selectedPaymentItemID.Contains(x.IDItem.ToString())).ToList();
                            paymentPlan.PaymentItem = newPaymenItemForPaymentPlan;

                            ctx.Entry(paymentPlan).State = EntityState.Modified;
                            retorno = ctx.SaveChanges();
                        }
                    }
                }

                if (retorno >= 0)
                    oPaymentPlan = GetPaymentPlanByID((int)paymentPlan.IDPlan);

                return oPaymentPlan;
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
