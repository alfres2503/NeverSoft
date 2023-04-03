using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IRepositoryArea
    {
        IEnumerable<Area> GetAreas();
        Area GetAreaByID(int id);
    }
}
