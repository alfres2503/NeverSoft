using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceUser: IServiceUser
    {
        private readonly IRepositoryUser _repositoryUser = new RepositoryUser();

        public User GetUserByID(int id)
        {
            return _repositoryUser.GetUserByID(id);
        }

        public IEnumerable<User> GetUsers()
        {
            return _repositoryUser.GetUsers();
        }
    }
}
