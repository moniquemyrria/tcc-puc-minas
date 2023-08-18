using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Core
{
    public class KRetorno<T> : KRetorno
        where T : class
    {
        public T TRetorno { get; set; }

        public static KRetorno<T> Failed(params KError[] errors)
        {
            var result = new KRetorno<T> { Sucesso = false, TRetorno=null };
            if (errors != null)
            {
                result.Errors.AddRange(errors);
            }
            return result;
        }
        public static KRetorno<T> Success(T entidade)
        {
            if (entidade == null)
                return Failed(new KError { Code = "", Description = "Entidade Null" });

            return new KRetorno<T> { Sucesso = true, TRetorno = entidade };
        }
    }

    public class KRetorno
    {
        private static readonly KRetorno _success = new KRetorno { Sucesso = true };
        private List<KError> _errors = new List<KError>();

        public static KRetorno Success => _success;

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="KError"/>s containing an errors
        /// that occurred during the SFC operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="KError"/>s.</value>
        public List<KError> Errors => _errors;

        public static KRetorno Failed(params KError[] errors)
        {
            var result = new KRetorno { Sucesso = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        public bool Sucesso { get; set; }
        public string Message{ get; set; }

        public override string ToString()
        {
            return Sucesso ?
                   "Succeeded" :
                   string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
        //public string Message { get; set; }
    }
}
