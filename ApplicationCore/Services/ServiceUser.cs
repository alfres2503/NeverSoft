using ApplicationCore.Utils;
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

        public User GetUserByID(long id)
        {
            return _repositoryUser.GetUserByID(id);
        }

        public User GetUserByIDForLogin(long id)
        {
            User oUser = _repositoryUser.GetUserByIDForLogin(id);
            // Desencriptar el password para presentarlo
            //oUser.Password = Cryptography.DecrypthAES(oUser.Password);


            return oUser;
        }

        public IEnumerable<User> GetUsers()
        {
            return _repositoryUser.GetUsers();
        }

        public User GetUsersForLogin(string email, string password)
        {
            // Encriptar el password para poder compararlo
            //string cryptPassword = Cryptography.EncrypthAES(password);
            return _repositoryUser.GetUsersForLogin(email, password);
        }

        public User Save(User user)
        {
            //Encriptar el password para guardarlo
            user.Password = Cryptography.EncrypthAES(user.Password);
            return _repositoryUser.Save(user);
        }
    }
}
