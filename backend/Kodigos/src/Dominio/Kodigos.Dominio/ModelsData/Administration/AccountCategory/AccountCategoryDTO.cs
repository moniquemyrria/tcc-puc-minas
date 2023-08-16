using Kodigos.Dominio.ModelsData.Administration.Account;
using Kodigos.Dominio.ModelsData.Administration.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Administration.AccountCategory
{
    public class AccountCategoryDTO : BaseDTO<long>
    {
        public string initials { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? dateUpdate { get; set; }

        public UsersDTO user { get; set; }

        [NotMapped]
        public string idUser  { get; set; }


    }
}
