using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Dominio.ModelsData
{
    public abstract class BaseDTO : BaseDTO<string>
    {
        public BaseDTO()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public abstract class BaseDTO<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key for this Base.
        /// </summary>
        public virtual TKey Id { get; set; }

        //public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        public override bool Equals(object obj)
        {
            var compareTo = obj as BaseDTO<TKey>;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        public static bool operator ==(BaseDTO<TKey> a, BaseDTO<TKey> b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }
        public static bool operator !=(BaseDTO<TKey> a, BaseDTO<TKey> b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 1024) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return GetType().Name + " [Id=" + Id + "]";
        }
    }
}
