using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IRepositoryNews
    {
        IEnumerable<News> GetNews();
        News GetNewsByID(int id);
        News Save(News news);
        IEnumerable<News> GetNewsByCategory(int idCategory);
    }
}
