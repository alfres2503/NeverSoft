using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceResidence : IServiceResidence
    {
        public IEnumerable<Residence> GetResidences()
        {
            IRepositoryResidence repository = new RepositoryResidence();
            return repository.GetResidences();
        }

        public Residence GetResidenceByID(int id)
        {
            IRepositoryResidence repository = new RepositoryResidence();
            return repository.GetResidenceByID(id);
        }

        public Residence GetResidenceByUser(long idUser)
        {
            IRepositoryResidence repository = new RepositoryResidence();
            return repository.GetResidenceByUser(idUser);
        }

        public Residence Save(Residence residence)
        {
            IRepositoryResidence repository = new RepositoryResidence();
            return repository.Save(residence);
        }
    }
}
