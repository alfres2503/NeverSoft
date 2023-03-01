using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RepositoryPaymentItem : IRepositoryPaymentItem
    {
        public void DeleteLibro(int id)
        {
            throw new NotImplementedException();
        }

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
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
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
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PaymentItem Save(PaymentItem paymentItem)
        {
            throw new NotImplementedException();
        }
    }
}
