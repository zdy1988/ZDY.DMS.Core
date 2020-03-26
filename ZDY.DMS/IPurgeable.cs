namespace ZDY.DMS
{
    /// <summary>
    /// Represents that the implemented classes can purge its data somehow.
    /// </summary>
    public interface IPurgeable
    {
        /// <summary>
        /// Purges the data contained in this instance.
        /// </summary>
        void Purge();
    }
}
