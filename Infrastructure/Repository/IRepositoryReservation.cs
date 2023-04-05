using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IRepositoryReservation
    {
        IEnumerable<Reservation> GetReservations();
        Reservation GetReservationByID(int id);
        IEnumerable<Reservation> GetReservationsByUser(long idUser);
        List<Reservation> GetReservationsByDate(int dayOfYear, int idArea);
        Reservation Save(Reservation reservation);
        void Approve(int id);
        void Deny(int id);
    }
}
