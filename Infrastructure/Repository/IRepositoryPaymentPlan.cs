using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
   public interface IRepositoryPaymentPlan
    {
        IEnumerable<PaymentPlan> GetPaymentPlans();
        PaymentPlan GetPaymentPlanByID(int id);
        PaymentPlan Save(PaymentPlan paymentPlan);
    }
}
