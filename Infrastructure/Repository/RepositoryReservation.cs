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
    public class RepositoryReservation : IRepositoryReservation
    {
        public IEnumerable<Reservation> GetReservations()
        {
            IEnumerable<Reservation> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    list = ctx.Reservation
                        .Include("Area")
                        .Include("User")
                        .ToList();
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

        public Reservation GetReservationByID(int id)
        {
            Reservation oReservation = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    oReservation = ctx.Reservation.
                        Where(r => r.IDReservation == id)
                        .Include("Area")
                        .Include("User")
                        .FirstOrDefault();

                }
                return oReservation;
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

        public IEnumerable<Reservation> GetReservationsByUser(long idUser)
        {
            IEnumerable<Reservation> list = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.Reservation.
                        Where(r => r.IDUser == idUser)
                        .Include("Area")
                        .Include("User")
                        .ToList();

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

        public List<Reservation> GetReservationsByDate(int dayOfYear, int idArea)
        {
            List<Reservation> list = null;
            List<Reservation> auxList = new List<Reservation>();
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    list = ctx.Reservation
                    .Where(r => r.Approved != false && r.IDArea == idArea)
                    .ToList();

                    foreach (Reservation r in list)
                    {
                        if (r.Start.DayOfYear == dayOfYear)
                        {
                            auxList.Add(r);
                        }
                    }
                }
                return auxList;
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

        public Reservation Save(Reservation reservation)
        {
            int retorno = 0;
            Reservation oReservation = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oReservation = GetReservationByID((int)reservation.IDReservation);

                    if (oReservation == null)
                    {
                        ctx.Reservation.Add(reservation);
                        
                        retorno = ctx.SaveChanges();
                        
                    }
                    else
                    {
                        
                        ctx.Reservation.Add(reservation);
                        ctx.Entry(reservation).State = EntityState.Modified;
                        retorno = ctx.SaveChanges();
                    }
                }

                if (retorno >= 0)
                    oReservation = GetReservationByID((int)reservation.IDReservation);

                return reservation;
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

        public void Approve(int id)
        {
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    Reservation reservation = GetReservationByID((int)id);

                    reservation.Approved = true;

                    ctx.Entry(reservation).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
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

        public void Deny(int id)
        {
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    Reservation reservation = GetReservationByID((int)id);

                    reservation.Approved = false;

                    ctx.Entry(reservation).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
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
