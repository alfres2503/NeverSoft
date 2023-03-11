using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceUser
    {
        IEnumerable<User> GetUsers();
        User GetUserByID(int id);
    }
}
