using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZDY.DMS.DataPermission;
using ZDY.DMS.KeyGeneration;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.AspNetCore.Mvc
{
    public class ApiDataServiceController<TKey, TEntity> : ApiController
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IRepositoryContext repositoryContext;
        private readonly IRepository<TKey, TEntity> repository;
        private readonly IKeyGenerator<TKey, TEntity> keyGenerator;

        public ApiDataServiceController(IRepositoryContext repositoryContext)
            : this(repositoryContext, new NullKeyGenerator<TKey>())
        { }

        public ApiDataServiceController(IRepositoryContext repositoryContext, IKeyGenerator<TKey, TEntity> keyGenerator)
        {
            this.repositoryContext = repositoryContext;
            this.repository = repositoryContext.GetRepository<TKey, TEntity>();
            this.keyGenerator = keyGenerator;
        }

        protected IRepositoryContext RepositoryContext => this.repositoryContext;

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

            var entity = await this.repository.FindByKeyAsync(id);

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

            var entity = await this.repository.FindAsync(searchModel.GetQueryModel());

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
                return await this.repository.FindAllAsync(searchModel.PageIndex, searchModel.PageSize, searchModel.OrderField, searchModel.IsAsc);
            }
            else
            {
                return await this.repository.FindAllAsync(searchModel.GetQueryModel(), searchModel.PageIndex, searchModel.PageSize, searchModel.OrderField, searchModel.IsAsc);
            }
        }

        [HttpPost]
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new InvalidOperationException("The entity that is going to be created has not been specified.");
            }

            if (!entity.Id.Equals(default) && await this.repository.ExistsAsync(x => x.Id.Equals(entity.Id)))
            {
                throw new InvalidOperationException($"The entity already exists.");
            }

            this.BeforeAdd(entity);

            var generatedKey = this.keyGenerator.Generate(entity);

            if (!generatedKey.Equals(default))
            {
                entity.Id = generatedKey;
            }

            await this.repository.AddAsync(entity);

            await this.repositoryContext.CommitAsync();

            return entity;
        }

        protected virtual void BeforeAdd(TEntity entity) { }

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

            await this.repository.UpdateAsync(original);
            await this.repositoryContext.CommitAsync();

            return entity;
        }

        protected virtual void BeforeUpdate(TEntity original, TEntity entity)
        {
            AutoMapping(entity, original);
        }

        [HttpPost]
        public virtual async Task Delete(TKey id)
        {
            if (id.Equals(default))
            {
                throw new InvalidOperationException("Entity key has not been specified.");
            }

            if (!await this.repository.ExistsAsync(x => x.Id.Equals(id)))
            {
                throw new InvalidOperationException($"The entity does not exist.");
            }

            this.BeforeDelete(id);

            if (typeof(IDisabledEntity<TKey>).IsAssignableFrom(typeof(TEntity)))
            {
                var original = await this.Repository.FindByKeyAsync(id);

                this.BeforeDelete(original);

                ((IDisabledEntity<TKey>)original).IsDisabled = true;

                await this.repository.UpdateAsync(original);
            }
            else
            {
                await this.repository.RemoveByKeyAsync(id);
            }

            await this.repositoryContext.CommitAsync();
        }

        protected virtual void BeforeDelete(TKey id) { }

        protected virtual void BeforeDelete(TEntity original) { }


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
