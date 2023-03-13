using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RepositoryNewsCategory : IRepositoryNewsCategory
    {
        public IEnumerable<NewsCategory> GetNewsCategory()
        {
            IEnumerable<NewsCategory> lista = null;
            try
            {


                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.NewsCategory.ToList();


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

        public NewsCategory GetNewsCategoryByID(int id)
        {
            NewsCategory oNewsCategory = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    oNewsCategory = ctx.NewsCategory.
                        Where(l => l.IDCategory == id).
                        FirstOrDefault();

                }
                return oNewsCategory;
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
