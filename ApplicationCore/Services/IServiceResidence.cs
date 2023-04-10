using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceResidence
    {
        IEnumerable<Residence> GetResidences();
        Residence GetResidenceByID(int id);
        Residence GetResidenceByUser(long idUser);
    }
}
