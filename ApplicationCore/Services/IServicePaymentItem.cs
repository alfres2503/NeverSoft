using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IServicePaymentItem
    {
        IEnumerable<PaymentItem> GetPaymentItem();
        PaymentItem GetPaymentItemByID(int id);
        void DeleteLibro(int id);
        PaymentItem Save(PaymentItem paymentItem);
    }
}
