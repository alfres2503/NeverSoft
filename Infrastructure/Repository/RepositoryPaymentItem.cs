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
            int retorno = 0;
            PaymentItem oPaymentItem = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oPaymentItem = GetPaymentItemByID((int)paymentItem.IDItem);
               

                if (oPaymentItem == null)
                {

                   
                    //Insertar 
                    ctx.PaymentItem.Add(paymentItem);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                    //retorna número de filas afectadas
                }
                else
                {
                   
                    //Actualizar Libro
                    ctx.PaymentItem.Add(paymentItem);
                    ctx.Entry(paymentItem).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();

                    
                }
            }

            if (retorno >= 0)
                oPaymentItem = GetPaymentItemByID((int)paymentItem.IDItem);

            return oPaymentItem;
        }
    }
}
