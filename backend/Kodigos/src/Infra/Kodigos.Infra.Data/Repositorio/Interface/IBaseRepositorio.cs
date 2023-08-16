
using Kodigos.Dominio.ModelsData;
using Kodigos.Infra.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Data.Repositorio.Interface
{
    public interface IBaseRepositorio<TEntidade, TKey>
        where TEntidade : BaseDTO<TKey>
        where TKey : IEquatable<TKey>
    {
        bool AutoSaveChanges { get; set; }
        Task<int> SaveChangesAsync();
        int SaveChanges();
        KPagedList<TEntidade> ToList(int page, int pageSize);
        //ICollection<TEntidade> ToList();
        IQueryable<TEntidade> List();

        /// <summary>
        /// Gets the Entidade identifier for the specified <paramref name="Id"/>.
        /// </summary>
        /// <param name="Id">The Entidade whose identifier should be retrieved.</param>
        KRetorno<TEntidade> FindById(TKey Id);

        /// <summary>
        /// Creates the specified <paramref name="Entidade"/> in the Entidade store.
        /// </summary>
        /// <param name="Entidade">The Entidade to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="SFCResult"/> of the creation operation.</returns>
        Task<KRetorno<TEntidade>> CreateAsync(TEntidade Entidade, CancellationToken cancellationToken);

        /// <summary>
        /// Creates the specified <paramref name="Entidade"/> in the Entidade store.
        /// </summary>
        /// <param name="Entidade">The Entidade to create.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="KRetorno"/> of the creation operation.</returns>
        Task<KRetorno<TEntidade>> UpdateAsync(TEntidade entidade, CancellationToken cancellation);

        Task<KRetorno> DeleteAsync(TEntidade entidade, CancellationToken cancellation);

    }
}
