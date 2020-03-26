using ZDY.DMS.Messaging;

namespace ZDY.DMS.Commands
{
    /// <summary>
    /// Represents that the implemented classes are commands.
    /// </summary>
    /// <seealso cref="ZDY.DMS.Messaging.IMessage" />
    public interface ICommand : IMessage
    {
    }
}
