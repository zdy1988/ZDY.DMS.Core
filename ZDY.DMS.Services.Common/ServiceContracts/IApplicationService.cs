using System;
using System.Collections.Generic;
using ZDY.DMS.Repositories;

namespace ZDY.DMS.Services.Common.ServiceContracts
{
    public interface IApplicationService
    {
        /// <summary>
        /// 处理简单的创建逻辑。
        /// </summary>
        /// <typeparam name="TDataObjectList">包含数据传输对象的列表类型，比如<see cref="UserDataObjectList"/>等。</typeparam>
        /// <typeparam name="TDataObject">数据传输对象类型，比如<see cref="UserDataObject"/>等。</typeparam>
        /// <typeparam name="TKey">聚合根主键类型。</typeparam>
        /// <typeparam name="TEntity">聚合根类型。</typeparam>
        /// <param name="dataTransferObjects">包含了一系列数据传输对象的列表实例。</param>
        /// <param name="repository">用于特定聚合根类型的仓储实例。</param>
        /// <param name="processDto">指定用于处理Data Transferring Object类型的函数。</param>
        /// <param name="processAggregateRoot">指定用于处理聚合根类型的函数。</param>
        /// <returns>包含了已创建的聚合的数据的列表。</returns>
        TDataObjectList PerformCreateObjects<TDataObjectList, TDataObject, TKey, TEntity>(
            TDataObjectList dataTransferObjects,
            IRepository<TKey, TEntity> repository,
            Action<TDataObject> processDto = null,
            Action<TEntity> processAggregateRoot = null)
            where TDataObjectList : List<TDataObject>, new()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>;

        /// <summary>
        /// 简单的聚合更新操作。
        /// </summary>
        /// <typeparam name="TDataObjectList">包含数据传输对象的列表类型，比如<see cref="UserDataObjectList"/>等。</typeparam>
        /// <typeparam name="TDataObject">数据传输对象类型，比如<see cref="UserDataObject"/>等。</typeparam>
        /// <typeparam name="TKey">聚合根主键类型。</typeparam>
        /// <typeparam name="TEntity">聚合根类型。</typeparam>
        /// <param name="dataTransferObjects">包含了一系列数据传输对象的列表实例。</param>
        /// <param name="repository">用于特定聚合根类型的仓储实例。</param>
        /// <param name="idFieldFunc">用于获取数据传输对象唯一标识值的回调函数。</param>
        /// <param name="fieldUpdateAction">用于执行聚合更新的回调函数。</param>
        /// <returns>包含了已更新的聚合的数据的列表。</returns>
        TDataObjectList PerformUpdateObjects<TDataObjectList, TDataObject, TKey, TEntity>(
            TDataObjectList dataTransferObjects,
            IRepository<TKey, TEntity> repository,
            Func<TDataObject, TKey> idFieldFunc,
            Action<TEntity, TDataObject> fieldUpdateAction)
            where TDataObjectList : List<TDataObject>, new()
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>;

        /// <summary>
        /// 简单的删除聚合根的操作。
        /// </summary>
        /// <typeparam name="TKey">需要删除的聚合根主键类型。</typeparam>
        /// <typeparam name="TEntity">需要删除的聚合根的类型。</typeparam>
        /// <param name="ids">需要删除的聚合根的ID值列表。</param>
        /// <param name="repository">应用于指定聚合根类型的仓储实例。</param>
        /// <param name="preDelete">在指定聚合根被删除前，对所需删除的聚合根的ID值进行处理的回调函数。</param>
        /// <param name="postDelete">在指定聚合根被删除后，对所需删除的聚合根的ID值进行处理的回调函数。</param>
        void PerformDeleteObjects<TKey, TEntity>(List<TKey> ids,
            IRepository<TKey, TEntity> repository,
            Action<TKey> preDelete = null,
            Action<TKey> postDelete = null)
            where TKey : IEquatable<TKey>
            where TEntity : class, IEntity<TKey>;
    }
}
