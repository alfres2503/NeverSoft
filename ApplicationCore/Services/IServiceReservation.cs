using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServiceReservation
    {
        IEnumerable<Reservation> GetReservations();
        Reservation GetReservationByID(int id);
        IEnumerable<Reservation> GetReservationsByUser(long idUser);
        List<Reservation> GetReservationsByDate(int date);
        Reservation Save(Reservation reservation);
        void Approve(int id);
        void Deny(int id);
    }
}