using Kodigos.Dominio.ModelsData.Administration.Account;
using Kodigos.Dominio.ModelsData.Administration.RevenueCategory;
using Kodigos.Dominio.ModelsData.Administration.TypePerson;
using Kodigos.Dominio.ModelsData.Administration.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Revenue
{
    public class RevenueDTO : BaseDTO<long>
    {
        public double value { get; set; }
        public string? description { get; set; }
        public string? obs { get; set; }
        public string revenueReceipt { get; set; }
        public string revenueStatus { get; set; }
        public bool isActive { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime? dateUpdate { get; set; }

        public UsersDTO user { get; set; }
        public RevenueCategoryDTO revenueCategory { get; set; }
        public AccountDTO account { get; set; }
        public SupplierDTO supplier { get; set; }

        [NotMapped]
        public string idUser  { get; set; }

        [NotMapped]
        public long idRevenueCategory { get; set; }

        [NotMapped]
        public long idAccount { get; set; }

        [NotMapped]
        public long idTypePerson { get; set; }

        [NotMapped]
        public string colorRevenueCategory { get; set; }

        [NotMapped]
        public string dateCreatedFormat { get; set; }

        [NotMapped]
        public double totalSumRevenue { get; set; }

    }
}
