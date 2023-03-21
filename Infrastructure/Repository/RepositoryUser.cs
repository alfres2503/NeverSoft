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
    public class RepositoryUser : IRepositoryUser
    {
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> lista = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.User.ToList();

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

        public User GetUserByID(int id)
        {
            User oUser = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    oUser = ctx.User.
                        Where(l => l.IDUser == id)
                        .FirstOrDefault();

                }
                return oUser;
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

        public User Save(User user)
        {
            int retorno = 0;
            User oUser = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oUser = GetUserByIDForLogin(user.IDUser);
                    if (oUser == null)
                    {
                        ctx.User.Add(user);
                    }
                    else
                    {
                        ctx.Entry(user).State = EntityState.Modified;
                    }
                    retorno = ctx.SaveChanges();
                }
                if (retorno >= 0) oUser = GetUserByIDForLogin(user.IDUser);
                return oUser;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public User GetUsersForLogin(string email, string password)
        {
            User oUser = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oUser = ctx.User.
                     Where(p => p.Email.Equals(email) && p.Password == password).
                    FirstOrDefault<User>();
                }
                if (oUser != null)
                    oUser = GetUserByIDForLogin(oUser.IDUser);
                return oUser;
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

        public User GetUserByIDForLogin(long id)
        {
            User user = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    user = ctx.User.
                     Include("UserRole").
                    Where(p => p.IDUser == id).
                    FirstOrDefault<User>();
                }
                return user;
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
