using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IRepositoryUser
    {
        IEnumerable<User> GetUsers();
        User GetUserByID(int id);
    }
}
