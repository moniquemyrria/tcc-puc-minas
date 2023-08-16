using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Core
{
    public class KError
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return Code + "|" + Description;
        }
    }
}
