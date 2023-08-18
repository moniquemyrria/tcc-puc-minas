using Kodigos.Dominio.ModelsData.Administration.Account;
using Kodigos.Dominio.ModelsData.Administration.ExpenseCategory;
using Kodigos.Dominio.ModelsData.Administration.PaymentMethods;
using Kodigos.Dominio.ModelsData.Administration.PaymentMethodsTypes;
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
    public class ExpensePaymentMethodsTypesDTO : BaseDTO<long>
    {
        public double? taxRate { get; set; }
        public double? totalPayment { get; set; }
        public double? trafficTicket { get; set; }
        public double? value { get; set; }
        public DateTime paymentDate { get; set; }

        public ExpenseDTO expense { get; set; }

     
        public PaymentMethodsDTO paymentMethods { get; set; }


        public PaymentMethodsTypesDTO paymentMethodsTypes { get; set; }
        public ExpenseInstallmentsDTO expenseInstallments { get; set; }


        [NotMapped]
        public long idExpense { get; set; }

        [NotMapped]
        public long idPaymentMethods { get; set; }

        [NotMapped]
        public long idPaymentMethodsTypes { get; set; }


    }
}
