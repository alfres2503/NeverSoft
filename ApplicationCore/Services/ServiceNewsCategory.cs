using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceNewsCategory : IServiceNewsCategory
    {
        public IEnumerable<NewsCategory> GetNewsCategory()
        {
            IRepositoryNewsCategory repository = new RepositoryNewsCategory();
            return repository.GetNewsCategory();
        }

        public NewsCategory GetNewsCategoryByID(int id)
        {
            IRepositoryNewsCategory repository = new RepositoryNewsCategory();
            return repository.GetNewsCategoryByID(id);
        }
    }
}
