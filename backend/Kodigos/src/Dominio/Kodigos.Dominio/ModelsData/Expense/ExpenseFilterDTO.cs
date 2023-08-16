using Kodigos.Dominio.ModelsData.Administration.Account;
using Kodigos.Dominio.ModelsData.Administration.ExpenseCategory;
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
    public class ExpenseFilterDTO
    {
        public DateTime? dateInitial { get; set; }
        public DateTime? dateFinal { get; set; }
        public string expenseStatus { get; set; }



        public ExpenseCategoryDTO? expenseCategory { get; set; }
        public AccountDTO? account { get; set; }
        public SupplierDTO? typePerson { get; set; }

    }
}
