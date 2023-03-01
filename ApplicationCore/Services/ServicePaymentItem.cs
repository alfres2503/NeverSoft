using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServicePaymentItem : IServicePaymentItem
    {
        public void DeleteLibro(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PaymentItem> GetPaymentItem()
        {
            IRepositoryPaymentItem repository = new RepositoryPaymentItem();
            return repository.GetPaymentItem();
        }

        public PaymentItem GetPaymentItemByID(int id)
        {
            IRepositoryPaymentItem repository = new RepositoryPaymentItem();
            return repository.GetPaymentItemByID(id);
        }

        public PaymentItem Save(PaymentItem paymentItem)
        {
            IRepositoryPaymentItem repository = new RepositoryPaymentItem();
            return repository.Save(paymentItem);
        }
    }
}
