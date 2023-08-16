using Kodigos.Dominio.ModelsData.Administration.Account;
using Kodigos.Dominio.ModelsData.Administration.PaymentMethodsTypes;
using Kodigos.Dominio.ModelsData.Administration.Users;
using Kodigos.Dominio.ModelsData.Expense;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Administration.PaymentMethods
{
    public class PaymentMethodsDTO : BaseDTO<long>
    {
        public string name { get; set; }
        public bool acceptInstallment { get; set; }
        public bool isActive { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? dateUpdate { get; set; }

        public UsersDTO user { get; set; }
        public AccountDTO account { get; set; }

        [JsonIgnore]
        public List<ExpenseDTO> expense { get; set; }

        [NotMapped]
        public string idUser  { get; set; }

        [NotMapped]
        public long idAccount { get; set; }

        public List<PaymentMethodsTypesDTO> paymentMethodsTypes { get; set; }

        [JsonIgnore]
        public List<ExpensePaymentMethodsTypesDTO> expensePaymentMethodsTypes { get; set; }



    }
}
