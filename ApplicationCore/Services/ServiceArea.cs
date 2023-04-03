using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceArea : IServiceArea
    {
        private readonly IRepositoryArea _repositoryArea = new RepositoryArea();

        public IEnumerable<Infrastructure.Models.Area> GetAreas()
        {
            return _repositoryArea.GetAreas();
        }
        public Infrastructure.Models.Area GetAreaByID(int id)
        {
            return _repositoryArea.GetAreaByID(id);
        }
    }
}
