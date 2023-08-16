using Kodigos.Dominio.ModelsData.Administration.Account;
using Kodigos.Dominio.ModelsData.Administration.Users;
using Kodigos.Dominio.ModelsData.Expense;
using Kodigos.Dominio.ModelsData.Revenue;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Administration.TypePerson
{
    public class SupplierDTO : BaseDTO<long>
    {
       
        public string name { get; set; }
        public string typePerson { get; set; }
        public bool isActive { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? dateUpdate { get; set; }

        public UsersDTO user { get; set; }

        [JsonIgnore]
        public List<ExpenseDTO> expense { get; set; }

        [NotMapped]
        public string idUser  { get; set; }

        [JsonIgnore]
        public List<RevenueDTO> revenue { get; set; }
    }
}
