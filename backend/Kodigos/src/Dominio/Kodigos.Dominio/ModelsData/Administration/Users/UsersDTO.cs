
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Administration.Users
{
    public class UsersDTO : IdentityUser<string>
    {
        public bool isActive { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? dateUpdate { get; set; }

        public string name { get; set; }
        public DateTime? lastAccessDate { get; set; }

        [NotMapped]
        public string passwordRegister { get; set; }

    }
}
