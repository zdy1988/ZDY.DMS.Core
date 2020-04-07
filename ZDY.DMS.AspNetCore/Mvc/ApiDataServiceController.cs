using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;
using ZDY.DMS.AspNetCore.Module;

namespace ZDY.DMS.AspNetCore.Mvc
{
    public abstract class ApiDataServiceController<TKey, TEntity, TServiceModule> : ApiController<TServiceModule>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TServiceModule: IServiceModule
    {
        private readonly IRepository<TKey, TEntity> repository;
        private readonly IKeyGenerator<TKey, TEntity> keyGenerator;

        public ApiDataServiceController(Func<Type, IRepositoryContext> repositoryContextFactory)
            : this(repositoryContextFactory, new NullKeyGenerator<TKey>())
        { }

        public ApiDataServiceController(Func<Type, IRepositoryContext> repositoryContextFactory, IKeyGenerator<TKey, TEntity> keyGenerator)
            : base(repositoryContextFactory)
        {
            this.repository = this.GetRepository<TKey, TEntity>();
            this.keyGenerator = keyGenerator;
        }

        protected IRepository<TKey, TEntity> Repository => this.repository;

        protected IKeyGenerator<TKey, TEntity> KeyGenerator => this.keyGenerator;

        protected int PageSize
        {
            get
            {
                return 20;
            }
        }

        [HttpPost]
        public virtual async Task<TEntity> FindByKey(TKey id)
        {
            if (id.Equals(default))
            {
                throw new InvalidOperationException("Entity key has not been specified.");
            }

            var entity = await this.Repository.FindByKeyAsync(id);

            if (entity == null)
            {
                throw new InvalidOperationException($"The entity does not exist.");
            }

            return entity;
        }

        [HttpPost]
        public virtual async Task<TEntity> Find(SearchModel searchModel)
        {
            if (searchModel.GetQueryModel().Items.Count <= 0)
            {
                throw new InvalidOperationException("The searchModel items has not been specified.");
            }

            var entity = await this.Repository.FindAsync(searchModel.GetQueryModel());

            if (entity == null)
            {
                throw new InvalidOperationException($"The entity does not exist.");
            }

            return entity;
        }

        [HttpPost]
        public virtual async Task<Tuple<IEnumerable<TEntity>, int>> Search(SearchModel searchModel)
        {
            if (searchModel.PageIndex <= 0)
            {
                throw new ArgumentOutOfRangeException("The page index should be greater than 0.");
            }

            if (searchModel.PageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("The page size should be greater than 0.");
            }

            if (string.IsNullOrEmpty(searchModel.OrderField))
            {
                throw new ArgumentOutOfRangeException("The orderField has not been specified.");
            }

            if (searchModel.GetQueryModel().Items.Count <= 0)
            {
                return await this.Repository.FindAllAsync(searchModel.PageIndex, searchModel.PageSize, searchModel.OrderField, searchModel.IsAsc);
            }
            else
            {
                return await this.Repository.FindAllAsync(searchModel.GetQueryModel(), searchModel.PageIndex, searchModel.PageSize, searchModel.OrderField, searchModel.IsAsc);
            }
        }

        [HttpPost]
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new InvalidOperationException("The entity that is going to be created has not been specified.");
            }

            if (!entity.Id.Equals(default) && await this.Repository.ExistsAsync(x => x.Id.Equals(entity.Id)))
            {
                throw new InvalidOperationException($"The entity already exists.");
            }

            this.BeforeAdd(entity);

            var generatedKey = this.keyGenerator.Generate(entity);

            if (!generatedKey.Equals(default))
            {
                entity.Id = generatedKey;
            }

            await this.Repository.AddAsync(entity);

            await this.RepositoryContext.CommitAsync();

            this.AfterAdd(entity);

            return entity;
        }

        protected virtual void BeforeAdd(TEntity entity) { }

        protected virtual void AfterAdd(TEntity entity) { }

        [HttpPost]
        public virtual async Task<TEntity> Update(TEntity entity)
        {
            if (entity.Id.Equals(default))
            {
                throw new InvalidOperationException("Entity key has not been specified.");
            }

            if (entity == null)
            {
                throw new InvalidOperationException("The entity that is going to be updated has not been specified.");
            }

            var original = await this.Repository.FindByKeyAsync(entity.Id);

            if (original == null)
            {
                throw new InvalidOperationException($"The entity does not exist.");
            }

            this.BeforeUpdate(original, entity);

            await this.Repository.UpdateAsync(original);
            await this.RepositoryContext.CommitAsync();

            this.AfterUpdate(original);

            return entity;
        }

        protected virtual void BeforeUpdate(TEntity original, TEntity entity)
        {
            AutoMapping(entity, original);
        }

        protected virtual void AfterUpdate(TEntity original) { }

        [HttpPost]
        public virtual async Task Delete(TKey id)
        {
            if (id.Equals(default))
            {
                throw new InvalidOperationException("Entity key has not been specified.");
            }

            if (!await this.Repository.ExistsAsync(x => x.Id.Equals(id)))
            {
                throw new InvalidOperationException($"The entity does not exist.");
            }

            this.BeforeDelete(id);

            if (typeof(IDisabledEntity<TKey>).IsAssignableFrom(typeof(TEntity)))
            {
                var original = await this.Repository.FindByKeyAsync(id);

                this.BeforeDelete(original);

                ((IDisabledEntity<TKey>)original).IsDisabled = true;

                await this.Repository.UpdateAsync(original);

                await this.RepositoryContext.CommitAsync();

                this.AfterDelete(original);
            }
            else
            {
                await this.Repository.RemoveByKeyAsync(id);

                await this.RepositoryContext.CommitAsync();
            }

            this.AfterDelete(id);
        }

        protected virtual void BeforeDelete(TKey id) { }

        protected virtual void BeforeDelete(TEntity original) { }

        protected virtual void AfterDelete(TKey id) { }

        protected virtual void AfterDelete(TEntity original) { }


        /// <summary>
        /// 简单的对象间的赋值
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        protected static void AutoMapping(TEntity source, TEntity target)
        {
            Type type = typeof(TEntity);

            string[] basePropertyNames = typeof(IEntity<TKey>).GetProperties().Select(t => t.Name).ToArray();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                if (!basePropertyNames.Contains(property.Name))
                {
                    PropertyInfo targetProperty = type.GetProperty(property.Name);

                    object value = property.GetValue(source, null);

                    if (targetProperty != null && value != null)
                    {
                        targetProperty.SetValue(target, value, null);
                    }
                }
            }
        }
    }
}
