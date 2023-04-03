using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceArea
    {
        IEnumerable<Area> GetAreas();
        Area GetAreaByID(int id);
    }
}
