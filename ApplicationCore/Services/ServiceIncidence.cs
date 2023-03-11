using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceIncidence: IServiceIncidence
    {
        private IRepositoryIncidence _repositoryIncidence = new RepositoryIncidence();

        public IEnumerable<Incidence> GetIncidences()
        {
            return _repositoryIncidence.GetIncidences();
        }

        public Incidence GetIncidenceByID(int id)
        {
            return _repositoryIncidence.GetIncidenceByID(id);
        }

        public Incidence Save(Incidence incidence)
        {
            return _repositoryIncidence.Save(incidence);
        }
    }
}
