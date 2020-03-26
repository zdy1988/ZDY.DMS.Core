﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZDY.DMS.Snapshots
{
    /// <summary>
    /// Represents that the implemented classes are snapshot providers
    /// that will determine if the snapshot feature should be enabled
    /// and how could the snapshots be taken and used.
    /// </summary>
    public interface ISnapshotProvider
    {
        bool Enabled { get; }

        //bool ShouldSaveSnapshot<TKey, TAggregateRoot>(TAggregateRoot aggregateRoot)
        //    where TKey : IEquatable<TKey>
        //    where TAggregateRoot : class, IAggregateRootWithEventSourcing<TKey>;

        void SaveSnapshot(ISnapshot snapshot);

        Task SaveSnapshotAsync(ISnapshot snapshot, CancellationToken cancellationToken = default(CancellationToken));

        (bool, ISnapshot) CheckSnapshot<TKey, TAggregateRoot>(TKey key, long version);

        Task<(bool, ISnapshot)> CheckSnapshotAsync<TKey, TAggregateRoot>(TKey key, long version, CancellationToken cancellationToken = default(CancellationToken));

        //ISnapshot GetLatestSnapshot<TKey, TAggregateRoot>(TKey id, long version)
        //    where TKey : IEquatable<TKey>
        //    where TAggregateRoot : class, IAggregateRootWithEventSourcing<TKey>;

        //Task<ISnapshot> GetLatestSnapshotAsync<TKey, TAggregateRoot>(TKey id, long version, CancellationToken cancellationToken = default(CancellationToken))
        //    where TKey : IEquatable<TKey>
        //    where TAggregateRoot : class, IAggregateRootWithEventSourcing<TKey>;
    }
}
