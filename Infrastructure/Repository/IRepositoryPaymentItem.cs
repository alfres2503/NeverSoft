﻿using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IRepositoryPaymentItem
    {
        IEnumerable<PaymentItem> GetPaymentItem();
        PaymentItem GetPaymentItemByID(int id);
        PaymentItem Save(PaymentItem paymentItem);
    }
}
