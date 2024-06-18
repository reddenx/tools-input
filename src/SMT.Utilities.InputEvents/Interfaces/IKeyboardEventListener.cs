using SMT.Utilities.InputEvents.HardwareEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Utilities.InputEvents.Interfaces
{
    public delegate KeyboardEventResult KeyboardEventHandler(KeyboardEventArgs eventArgs);

    public interface IKeyboardEventListener : IDisposable
    {
        void StartListening(KeyboardEventHandler handler);
        void StopListening();
    }
}
