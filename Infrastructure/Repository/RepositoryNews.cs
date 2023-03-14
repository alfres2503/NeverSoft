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
    public class RepositoryNews : IRepositoryNews
    {
        public void DeleteNews(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<News> GetNews()
        {
            IEnumerable<News> lista = null;
            try
            {

                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    lista = ctx.News.Include("NewsCategory").ToList();


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

        public News GetNewsByID(int id)
        {
            News oNews = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;

                    oNews = ctx.News.
                        Where(l => l.IDNews == id).
                        FirstOrDefault();

                }
                return oNews;
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

        public IEnumerable<News> GetNewsByCategory(int idCategory)
        {
            IEnumerable<News> oNews = null;
            try
            {
                using (MyContext ctx = new MyContext())
                {
                    ctx.Configuration.LazyLoadingEnabled = false;
                    oNews = ctx.News.
                        Where(n => n.IDCategory == idCategory)
                        .ToList();

                }
                return oNews;
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

        public News Save(News news)
        {
            int retorno = 0;
            News oNews = null;

            using (MyContext ctx = new MyContext())
            {
                ctx.Configuration.LazyLoadingEnabled = false;
                oNews = GetNewsByID((int)news.IDNews);

                if (oNews == null)
                {
                    ctx.News.Add(news);
                    //SaveChanges
                    //guarda todos los cambios realizados en el contexto de la base de datos.
                    retorno = ctx.SaveChanges();
                    //retorna número de filas afectadas
                }
                else
                {
                    //Actualizar 
                    ctx.News.Add(news);
                    ctx.Entry(news).State = EntityState.Modified;
                    retorno = ctx.SaveChanges();
                }
            }

            if (retorno >= 0)
                oNews = GetNewsByID((int)news.IDNews);

            return oNews;
        }
    }
}
