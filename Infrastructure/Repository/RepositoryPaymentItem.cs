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
    public class RepositoryPaymentItem : IRepositoryPaymentItem
    {

        public IEnumerable<PaymentItem> GetPaymentItem()
        {
            IEnumerable<PaymentItem> lista = null;
            try
            {


                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    
                    lista = ctx.PaymentItem.ToList();

                   
                }
                return lista;
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

        public PaymentItem GetPaymentItemByID(int id)
        {
            PaymentItem oPaymentItem = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    
                    oPaymentItem = ctx.PaymentItem.
                        Where(l => l.IDItem == id).
                        FirstOrDefault();

                }
                return oPaymentItem;
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

        public PaymentItem Save(PaymentItem paymentItem)
        {
            int retorno = 0;
            PaymentItem oPaymentItem = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oPaymentItem = GetPaymentItemByID((int)paymentItem.IDItem);


                    if (oPaymentItem == null)
                    {



                        ctx.PaymentItem.Add(paymentItem);

                        retorno = ctx.SaveChanges();

                    }
                    else
                    {


                        ctx.PaymentItem.Add(paymentItem);
                        ctx.Entry(paymentItem).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();


                    }
                }

                if (retorno >= 0)
                    oPaymentItem = GetPaymentItemByID((int)paymentItem.IDItem);

                return oPaymentItem;
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
