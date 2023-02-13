using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IRepositoryResidence
    {
        IEnumerable<Residence> GetResidences();
        Residence GetResidenceByID(int id);
    }
}
