using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceUserRole : IServiceUserRole
    {
        public IEnumerable<UserRole> GetUserRoles()
        {
            IRepositoryUserRole repository = new RepositoryUserRole();
            return repository.GetUserRoles();
        }
    }
}
