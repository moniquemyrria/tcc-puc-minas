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
    public class RevenueFilterDTO
    {
        public DateTime? dateInitial { get; set; }
        public DateTime? dateFinal { get; set; }
        public string revenueReceipt { get; set; }
        public string revenueStatus { get; set; }



        public RevenueCategoryDTO? revenueCategory { get; set; }
        public AccountDTO? account { get; set; }
        public SupplierDTO? supplier { get; set; }

    }
}
