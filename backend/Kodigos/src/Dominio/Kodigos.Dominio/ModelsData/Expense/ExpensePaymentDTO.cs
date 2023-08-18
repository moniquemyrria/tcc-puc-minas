using Kodigos.Dominio.ModelsData.Administration.Account;
using Kodigos.Dominio.ModelsData.Administration.ExpenseCategory;
using Kodigos.Dominio.ModelsData.Administration.PaymentMethods;
using Kodigos.Dominio.ModelsData.Administration.TypePerson;
using Kodigos.Dominio.ModelsData.Administration.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Expense
{
    public class ExpensePaymentDTO : BaseDTO<long>
    {
        public long idInstallment { get; set; }
        public long idExpense { get; set; }
        public double amount { get; set; }
     
        public DateTime paymentDate { get; set; }

        public AccountDTO account { get; set; }
        public PaymentMethodsDTO? paymentMethods { get; set; }
      
    }
}
