
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Expense
{
    public class ExpenseInstallmentsDTO : BaseDTO<long>
    {
        public DateTime dueDate { get; set; }
        public long installment { get; set; }
        public string installmentDescription{ get; set; }
        public double value { get; set; }
        public bool isPaid { get; set; }
        public DateTime? paymentDate { get; set; }

        [JsonIgnore]
        public ExpenseDTO expense { get; set; }


        [NotMapped]
        public long idExpense { get; set; }

        [NotMapped]
        public string dueDateFormatter { get; set; }

        [NotMapped]
        public string valueFormatter { get; set; }

        [NotMapped]
        public string paymentDateFormatter { get; set; }


        [JsonIgnore]
        public List<ExpensePaymentMethodsTypesDTO> expensePaymentMethodsTypes { get; set; }
    }
}
