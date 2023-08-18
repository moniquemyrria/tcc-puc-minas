
using Kodigos.Dominio.ModelsData.Administration.PaymentMethods;
using Kodigos.Dominio.ModelsData.Expense;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Administration.PaymentMethodsTypes
{
    public class PaymentMethodsTypesDTO : BaseDTO<long>
    {
        public string description { get; set; }
        public bool isActive { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? dateUpdate { get; set; }

        [JsonIgnore]
        public PaymentMethodsDTO paymentMethods { get; set; }

        [JsonIgnore]
        public List<ExpensePaymentMethodsTypesDTO> expensePaymentMethodsTypes { get; set; }



        [NotMapped]
        public long idPaymentMethods { get; set; }

        [NotMapped]
        public double? value { get; set; } = null; // valor

        [NotMapped]
        public double? trafficTicket { get; set; } = null; //juros

        [NotMapped]
        public double? taxRate { get; set; } = null; //multa

        [NotMapped]
        public double? totalPayment { get; set; } = null; //total pago por metodo de pagamento


    }
}
