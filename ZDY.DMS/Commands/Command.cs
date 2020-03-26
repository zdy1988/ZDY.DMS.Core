using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZDY.DMS.Messaging;

namespace ZDY.DMS.Commands
{
    public abstract class Command : Message, ICommand
    {
    }
}
