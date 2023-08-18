using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData.Help
{
    public class HelpDTO : BaseDTO<long>
    {
        public string name { get; set; }
        public string email { get; set; }
        public string cpf { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public string typePerson { get; set; }
        public DateTime dateCreated { get; set; }
    }
}
