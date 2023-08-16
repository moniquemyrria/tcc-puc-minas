using Kodigos.Dominio.ModelsData.Administration.AccountCategory;
using Kodigos.Dominio.ModelsData.Administration.TypePerson;
using Kodigos.Dominio.ModelsData.Administration.Users;
using Kodigos.Dominio.ModelsData.Expense;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Administration.Account
{
    public class AccountDTO : BaseDTO<long>
    {
        public string name { get; set; }
        public string agency { get; set; }
        public string accountNumber { get; set; }

        public bool isActive { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? dateUpdate { get; set; }

        public UsersDTO user { get; set; }

        public AccountCategoryDTO accountCategory { get; set; }

        public SupplierDTO typePerson { get; set; }

        [JsonIgnore]
        public List<ExpenseDTO> expense { get; set; }

        [NotMapped]
        public string idUser  { get; set; }

        [NotMapped]
        public long idAccountCategory { get; set; }

        [NotMapped]
        public long idTypePerson { get; set; }

    }
}
