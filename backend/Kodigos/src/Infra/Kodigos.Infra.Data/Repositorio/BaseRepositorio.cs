
using Kodigos.Dominio.ModelsData;
using Kodigos.Infra.Core;
using Kodigos.Infra.Data.Contexto;
using Kodigos.Infra.Data.Repositorio.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kodigos.Infra.Data.Repositorio
{
    public class BaseRepositorio<TContext, TEntidade> : BaseRepositorio<TContext, TEntidade, string>, IBaseRepositorio<TEntidade, string>
        where TContext : KodigosContext
        where TEntidade : BaseDTO<string>
    {
        public BaseRepositorio(TContext context, ILogger<BaseRepositorio<TContext, TEntidade>> logger) : base(context, logger)
        {
        }
    }
    public class BaseRepositorio<TContext, TEntidade, TKey> : IBaseRepositorio<TEntidade, TKey>
        where TContext : KodigosContext
        where TEntidade : BaseDTO<TKey>
        where TKey : IEquatable<TKey>
    {
        protected readonly TContext Context;
        protected readonly DbSet<TEntidade> _Set;
        protected readonly ILogger<BaseRepositorio<TContext, TEntidade, TKey>> Logger;

        public BaseRepositorio(TContext context, ILogger<BaseRepositorio<TContext, TEntidade, TKey>> logger)
        {
            Context = context;
            Logger = logger;
            _Set = Context.Set<TEntidade>();
        }

        public bool AutoSaveChanges { get; set; } = true;
        protected virtual async Task<int> AutoSaveChangesAsync()
        {
            return AutoSaveChanges ? await Context.SaveChangesAsync() : 0;
        }

        public async Task<KRetorno<TEntidade>> CreateAsync(TEntidade Entidade, CancellationToken cancellationToken)
        {
            if (Entidade == null)
            {
                return KRetorno<TEntidade>.Failed(new KError { Code = nameof(BaseRepositorio<TContext, TEntidade, TKey>), Description = "Entidade não pode ser nula" });
            }
            try
            {
                Context.Set<TEntidade>().Add(Entidade);
                //Context.SaveChanges();
                await AutoSaveChangesAsync();
                return KRetorno<TEntidade>.Success(Entidade);
            }
            catch (Exception e)
            {
                Logger.LogError("", e);
                return KRetorno<TEntidade>.Failed(new KError { Code = nameof(BaseRepositorio<TContext, TEntidade, TKey>), Description = e.Message });
            }
        }


        public async Task<KRetorno> DeleteAsync(TEntidade Entidade, CancellationToken cancellation)
        {
            if (Entidade == null)
            {
                return KRetorno.Failed(new KError { Code = nameof(DeleteAsync), Description = "Entidade nao encontrada" });
            }

            try
            {
                Context.Set<TEntidade>().Remove(Entidade);
                //Context.SaveChanges();
                AutoSaveChangesAsync();
            }
            catch (Exception E)
            {
                Logger.LogError("", E);
                return KRetorno.Failed(new KError { Code = nameof(DeleteAsync), Description = E.Message });
            }

            return KRetorno.Success;
        }

        public virtual KRetorno<TEntidade> FindById(TKey Id)
        {
            TEntidade t = Context.Set<TEntidade>().Find(Id);
            if (t == null)
                return KRetorno<TEntidade>.Failed(new KError { Code = nameof(FindById), Description = "Entidade não encontrada com o ID Informado" });
            else
                return KRetorno<TEntidade>.Success(t);
        }

        public IQueryable<TEntidade> List()
        {
            return Context.Set<TEntidade>();
        }

        public int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return Context.SaveChangesAsync();
        }

        public KPagedList<TEntidade> ToList(int page, int pageSize)
        {
            var ret = new KPagedList<TEntidade>();
            var set = Context.Set<TEntidade>();

            ret.TotalCount = set.Count();
            if (pageSize == 0)
                ret.Data.AddRange(set.Skip((page - 1) * pageSize));
            else
                ret.Data.AddRange(set.Skip((page - 1) * pageSize).Take(pageSize));

            pageSize = pageSize == 0 ? 1 : pageSize;

            ret.PageSize = pageSize;
            ret.PageCount = (int)ret.TotalCount / pageSize;
            ret.PageCount += (ret.TotalCount % pageSize) > 0 ? 1 : 0;

            return ret;
        }

        public async Task<KRetorno<TEntidade>> UpdateAsync(TEntidade Entidade, CancellationToken cancellation)
        {
            if (Entidade == null)
            {
                throw new ArgumentNullException(nameof(Entidade));
            }

            Context.Set<TEntidade>().Update(Entidade);
            AutoSaveChangesAsync();
            return KRetorno<TEntidade>.Success(Entidade);
        }
    }
}
