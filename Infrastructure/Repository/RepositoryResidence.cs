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
    public class RepositoryResidence : IRepositoryResidence
    {
        public IEnumerable<Residence> GetResidences()
        {
            try
            {
                IEnumerable<Residence> list = null;
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                   
                    list = ctx.Residence
                        .Include("User")
                        .Include("PlanAssignment.PaymentPlan.PaymentItem")
                        .ToList<Residence>();
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

        public Residence GetResidenceByID(int id)
        {
            Residence oResidence = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oResidence = ctx.Residence
                        .Where(r => r.IDResidence == id)
                        .Include("User")
                        .Include("PlanAssignment.PaymentPlan.PaymentItem")
                        .FirstOrDefault();
                }
                return oResidence;
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
