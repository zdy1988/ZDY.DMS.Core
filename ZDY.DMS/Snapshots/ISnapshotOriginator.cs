namespace ZDY.DMS.Snapshots
{
    /// <summary>
    /// Represents that the implemented classes are the originators of a snapshot.
    /// </summary>
    public interface ISnapshotOriginator
    {
        /// <summary>
        /// Takes the snapshot of the current object.
        /// </summary>
        /// <returns>The snapshot of the current instance.</returns>
        ISnapshot TakeSnapshot();

        /// <summary>
        /// Restores the state of current object by using the specified snapshot.
        /// </summary>
        /// <param name="snapshot">The snapshot from which the state of current object is restored.</param>
        void RestoreSnapshot(ISnapshot snapshot);
    }
}
