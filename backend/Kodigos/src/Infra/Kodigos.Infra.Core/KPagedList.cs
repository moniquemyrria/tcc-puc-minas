using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Core
{
    public class KPagedList<T> where T : class
    {
        public KPagedList()
        {
            Data = new List<T>();
        }

        public List<T> Data { get; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
}
