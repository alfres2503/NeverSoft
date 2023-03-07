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

        public PaymentPlan Save(PaymentPlan paymentPlan, string[] selectedPaymentItems)
        {
            int retorno = 0;
            PaymentPlan oPaymentPlan = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oPaymentPlan = GetPaymentPlanByID((int)paymentPlan.IDPlan);
                IRepositoryPaymentItem _RepositoryPaymentItem = new RepositoryPaymentItem();

                if (oPaymentPlan == null)
                {

                    //Insertar
                    //Logica para agregar las categorias al libro
                    if (selectedPaymentItems != null)
                    {

                        paymentPlan.PaymentItem = new List<PaymentItem>();
                        foreach (var paymentI in selectedPaymentItems)
                        {
                            var paymentItemToAdd = _RepositoryPaymentItem.GetPaymentItemByID(int.Parse(paymentI));
                            ctx.PaymentItem.Attach(paymentItemToAdd); //sin esto, EF intentará crear una categoría
                            paymentPlan.PaymentItem.Add(paymentItemToAdd);// asociar a la categoría existente con el libro


                        }
                    }
                    //Insertar Libro
                    ctx.PaymentPlan.Add(paymentPlan);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                    //retorna número de filas afectadas
                }
                else
                {
                    //Registradas: 1,2,3
                    //Actualizar: 1,3,4

                    //Actualizar Libro
                    ctx.PaymentPlan.Add(paymentPlan);
                    ctx.Entry(paymentPlan).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();

                    //Logica para actualizar Categorias
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


    }
}
