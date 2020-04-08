using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ZDY.DMS.Repositories.EntityFramework
{
    public sealed class EntityFrameworkRepositoryContext : RepositoryContext<DbContext>
    {
        private bool disposed = false;

        public EntityFrameworkRepositoryContext(DbContext session) : base(session) { }

        protected override IRepository<TKey, TEntity> CreateRepository<TKey, TEntity>()
            => new EntityFrameworkRepository<TKey, TEntity>(this);

        public override void Commit()
        {
            this.Session.SaveChanges();
        }

        public override async Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {       
            await this.Session.SaveChangesAsync(cancellationToken);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.Session.Dispose();
                }

                disposed = true;
                base.Dispose(disposing);
            }
        }
    }
}
