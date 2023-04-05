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
                        
                        retorno = ctx.SaveChanges();
                    }
                    else
                    {
                        ctx.Incidence.Add(incidence);
                        ctx.Entry(incidence).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();
                        
                    }
                    if(retorno >= 0)
                        oIncidence = GetIncidenceByID((int)incidence.IDIncidence);
                }
                return oIncidence;
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
