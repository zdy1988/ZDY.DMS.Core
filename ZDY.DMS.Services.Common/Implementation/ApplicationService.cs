using AutoMapper;
using System;
using System.Collections.Generic;
using ZDY.DMS.Repositories;
using ZDY.DMS.Services.Common.ServiceContracts;

namespace ZDY.DMS.Services.Common.Implementation
{
    public class ApplicationService : IApplicationService
    {                                                                
        private readonly IMapper _mapper;

        public ApplicationService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// 处理简单的创建逻辑。
        /// </summary>
        /// <typeparam name="TDataObjectList">包含数据传输对象的列表类型，比如<see cref="UserDataObjectList"/>等。</typeparam>
        /// <typeparam name="TDataObject">数据传输对象类型，比如<see cref="UserDataObject"/>等。</typeparam>
        /// <typeparam name="TEntity">聚合根类型。</typeparam>
        /// <param name="dataTransferObjects">包含了一系列数据传输对象的列表实例。</param>
        /// <param name="repository">用于特定聚合根类型的仓储实例。</param>
        /// <param name="processDto">指定用于处理Data Transferring Object类型的函数。</param>
        /// <param name="processAggregateRoot">指定用于处理聚合根类型的函数。</param>
        /// <returns>包含了已创建的聚合的数据的列表。</returns>
        public TDataObjectList PerformCreateObjects<TDataObjectList, TDataObject, TKey, TEntity>(TDataObjectList dataTransferObjects,
            IRepository<TKey,TEntity> repository,
            Action<TDataObject> processDto = null,
            Action<TEntity> processAggregateRoot = null)
            where TDataObjectList : List<TDataObject>, new()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (dataTransferObjects == null)
                throw new ArgumentNullException("dataTransferObjects");
            if (repository == null)
                throw new ArgumentNullException("repository");
            TDataObjectList result = null;
            if (dataTransferObjects.Count > 0)
            {
                var ars = new List<TEntity>();
                result = new TDataObjectList();
                foreach (var dto in dataTransferObjects)
                {
                    if (processDto != null)
                        processDto(dto);
                    var ar = _mapper.Map<TDataObject, TEntity>(dto);
                    if (processAggregateRoot != null)
                        processAggregateRoot(ar);
                    ars.Add(ar);
                    repository.Add(ar);
                }
                repository.Context.Commit();
                ars.ForEach(ar => result.Add(_mapper.Map<TEntity, TDataObject>(ar)));
            }
            return result;
        }

        /// <summary>
        /// 简单的聚合更新操作。
        /// </summary>
        /// <typeparam name="TDataObjectList">包含数据传输对象的列表类型，比如<see cref="UserDataObjectList"/>等。</typeparam>
        /// <typeparam name="TDataObject">数据传输对象类型，比如<see cref="UserDataObject"/>等。</typeparam>
        /// <typeparam name="TEntity">聚合根类型。</typeparam>
        /// <param name="dataTransferObjects">包含了一系列数据传输对象的列表实例。</param>
        /// <param name="repository">用于特定聚合根类型的仓储实例。</param>
        /// <param name="idFieldFunc">用于获取数据传输对象唯一标识值的回调函数。</param>
        /// <param name="fieldUpdateAction">用于执行聚合更新的回调函数。</param>
        /// <returns>包含了已更新的聚合的数据的列表。</returns>
        public TDataObjectList PerformUpdateObjects<TDataObjectList, TDataObject, TKey, TEntity>(TDataObjectList dataTransferObjects,
            IRepository<TKey, TEntity> repository,
            Func<TDataObject, TKey> idFieldFunc,
            Action<TEntity, TDataObject> fieldUpdateAction)
            where TDataObjectList : List<TDataObject>, new()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (dataTransferObjects == null)
                throw new ArgumentNullException("dataTransferObjects");
            if (repository == null)
                throw new ArgumentNullException("repository");
            if (idFieldFunc == null)
                throw new ArgumentNullException("idFieldFunc");
            if (fieldUpdateAction == null)
                throw new ArgumentNullException("fieldUpdateAction");
            TDataObjectList result = null;
            if (dataTransferObjects.Count > 0)
            {
                result = new TDataObjectList();
                foreach (var dto in dataTransferObjects)
                {
                    var id = idFieldFunc(dto);
                    var ar = repository.FindByKey(id);
                    fieldUpdateAction(ar, dto);
                    repository.Update(ar);
                    result.Add(_mapper.Map<TEntity, TDataObject>(ar));
                }
                repository.Context.Commit();
            }
            return result;
        }

        /// <summary>
        /// 简单的删除聚合根的操作。
        /// </summary>
        /// <typeparam name="TEntity">需要删除的聚合根的类型。</typeparam>
        /// <param name="ids">需要删除的聚合根的ID值列表。</param>
        /// <param name="repository">应用于指定聚合根类型的仓储实例。</param>
        /// <param name="preDelete">在指定聚合根被删除前，对所需删除的聚合根的ID值进行处理的回调函数。</param>
        /// <param name="postDelete">在指定聚合根被删除后，对所需删除的聚合根的ID值进行处理的回调函数。</param>
        public void PerformDeleteObjects<TKey, TEntity>(List<TKey> ids,
            IRepository<TKey, TEntity> repository,
            Action<TKey> preDelete = null,
            Action<TKey> postDelete = null)
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>
        {
            if (ids == null)
                throw new ArgumentNullException("ids");
            if (repository == null)
                throw new ArgumentNullException("repository");
            foreach (var id in ids)
            {
                if (preDelete != null)
                    preDelete(id);
                var ar = repository.FindByKey( id);
                repository.Remove(ar);
                if (postDelete != null)
                    postDelete(id);
            }
            repository.Context.Commit();
        }
    }
}
