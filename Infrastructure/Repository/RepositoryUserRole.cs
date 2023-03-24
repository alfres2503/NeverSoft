using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RepositoryUserRole : IRepositoryUserRole
    {
        public IEnumerable<UserRole> GetUserRoles()
        {
            IEnumerable<UserRole> lista = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.UserRole.ToList();


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
    }
}
