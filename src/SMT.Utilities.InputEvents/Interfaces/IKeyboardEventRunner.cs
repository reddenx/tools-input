using SMT.Utilities.InputEvents.HardwareEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Utilities.InputEvents.Interfaces
{
    public interface IKeyboardEventRunner
    {
        void DoEvent(KeyboardEventArgs eventArgs);
    }
}
