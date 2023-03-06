using Infrastructure.Models;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public class ServicePaymentPlan : IServicePaymentPlan
    {
        public PaymentPlan GetPaymentPlanByID(int id)
        {
            IRepositoryPaymentPlan repository = new RepositoryPaymentPlan();
            return repository.GetPaymentPlanByID(id);
        }

        public IEnumerable<PaymentPlan> GetPaymentPlans()
        {
            IRepositoryPaymentPlan repository = new RepositoryPaymentPlan();
            return repository.GetPaymentPlans();
        }

        public PaymentPlan Save(PaymentPlan paymentPlan, string[] selectedPaymentItems)
        {
            IRepositoryPaymentPlan repository = new RepositoryPaymentPlan();
            return repository.Save(paymentPlan, selectedPaymentItems);
        }
    }
}
