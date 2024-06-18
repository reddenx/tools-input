using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Utilities.InputEvents.HardwareEvents
{
    public class KeyboardEventResult
    {
        public bool WasHandled { get; private set; }

        public KeyboardEventResult(bool wasHandled)
        {
            this.WasHandled = wasHandled;
        }
    }
}
