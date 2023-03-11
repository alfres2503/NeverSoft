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
    public class RepositoryIncidence : IRepositoryIncidence
    {
        public IEnumerable<Incidence> GetIncidences()
        {
            IEnumerable<Incidence> lista = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.Incidence.Include("User").ToList();


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

        public Incidence GetIncidenceByID(int id)
        {
            Incidence oIncidence = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    oIncidence = ctx.Incidence.
                        Where(l => l.IDIncidence == id)
                        .Include("User")
                        .FirstOrDefault();

                }
                return oIncidence;
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

        public Incidence Save(Incidence incidence)
        {
            int retorno = 0;
            Incidence oIncidence = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oIncidence = GetIncidenceByID((int)incidence.IDIncidence);

                    if (oIncidence == null)
                    {
                        ctx.Incidence.Add(incidence);
                        //ctx.Entry(incidence).State = System.Data.Entity.EntityState.Modified;
                        retorno = ctx.SaveChanges();
                    }
                    else
                    {
                        ctx.Incidence.Add(incidence);
                        ctx.Entry(incidence).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();
                        //ctx.Entry(incidence).State = System.Data.Entity.EntityState.Added;
                    }
                    if(retorno >= 0)
                        oIncidence = GetIncidenceByID((int)incidence.IDIncidence);
                }
                return oIncidence;
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
