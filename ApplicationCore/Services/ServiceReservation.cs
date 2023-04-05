using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServiceReservation : IServiceReservation
    {
        private readonly IRepositoryReservation _repositoryReservation = new RepositoryReservation();

        public void Approve(int id)
        {
            _repositoryReservation.Approve(id);
        }
        public void Deny(int id)
        {
            _repositoryReservation.Deny(id);
        }
        public Reservation GetReservationByID(int id)
        {
            return _repositoryReservation.GetReservationByID(id);
        }
        public List<Reservation> GetReservationsByDate(int date)
        {
            return _repositoryReservation.GetReservationsByDate(date);
        }
        public IEnumerable<Reservation> GetReservations()
        {
            return _repositoryReservation.GetReservations();
        }
        public IEnumerable<Reservation> GetReservationsByUser(long idUser)
        {
            return _repositoryReservation.GetReservationsByUser(idUser);
        }
        public Reservation Save(Reservation reservation)
        {
            return _repositoryReservation.Save(reservation);
        }
    }
}
