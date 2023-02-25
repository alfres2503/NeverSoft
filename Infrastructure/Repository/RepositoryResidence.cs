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
                    //list = ctx.Residence.Include("User").ToList<Residence>();
                    list = ctx.Residence
                        .Include("User")
                        .Include("PlanAssignment.PaymentPlan.PaymentItem")
                        .ToList<Residence>();
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
                throw dbEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
