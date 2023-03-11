using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IRepositoryIncidence
    {
        IEnumerable<Incidence> GetIncidences();
        Incidence GetIncidenceByID(int id);
        Incidence Save(Incidence incidence);
    }
}
