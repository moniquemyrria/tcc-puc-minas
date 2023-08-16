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
    public class ExpenseDTO : BaseDTO<long>
    {
        public double amount { get; set; }
        public string? description { get; set; }
        public string? obs { get; set; }
        public string? daysBetweenInstallments { get; set; }
        public DateTime dueDate { get; set; }
        public string? expenseStatus { get; set; }
        public string? numberInstallments { get; set; }
        public string? typeInstallment { get; set; }
        public string? expenseType { get; set; } // V - Variavel / Fixa
        public bool isActive { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? dateUpdate { get; set; }


        public UsersDTO user { get; set; }
        public AccountDTO account { get; set; }
        public ExpenseCategoryDTO expenseCategory { get; set; }
        public PaymentMethodsDTO? paymentMethods { get; set; }
        public SupplierDTO supplier { get; set; }

        public List<ExpenseInstallmentsDTO> expenseInstallments { get; set; }

        [JsonIgnore]
        public List<ExpensePaymentMethodsTypesDTO> expensePaymentMethodsTypes { get; set; }


        [NotMapped]
        public string idUser { get; set; }

        [NotMapped]
        public long idAccount { get; set; }

        [NotMapped]
        public long idExpenseCategory { get; set; }

        [NotMapped]
        public long idPaymentMethods { get; set; }

        [NotMapped]
        public long idTypePerson { get; set; }


        [NotMapped]
        public string dateCreatedFormat { get; set; }

        [NotMapped]
        public string? paymentDateFormat { get; set; } = "";

        [NotMapped]
        public string dueDateFormat { get; set; } = "";

        //installments

        [NotMapped]
        public long idInstallment { get; set; }

        [NotMapped]
        public string installmentDescription { get; set; }

        [NotMapped]
        public double valueInstallment { get; set; }

        [NotMapped]
        public bool isPaid { get; set; }

    }
}
