﻿using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceNews
    {
        IEnumerable<News> GetNews();
        News GetNewsByID(int id);
        void DeleteNews(int id);
        News Save(News news);
        IEnumerable<News> GetNewsByCategory(int idCategory);
    }
}
