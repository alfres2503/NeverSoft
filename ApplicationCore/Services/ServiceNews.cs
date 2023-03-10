using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceNews : IServiceNews
    {
        public void DeleteNews(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<News> GetNews()
        {
            IRepositoryNews repository = new RepositoryNews();
            return repository.GetNews();
        }

        public News GetNewsByID(int id)
        {
            IRepositoryNews repository = new RepositoryNews();
            return repository.GetNewsByID(id);
        }

        public News Save(News news)
        {
            IRepositoryNews repository = new RepositoryNews();
            return repository.Save(news);
        }
    }
}
