using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMT.Utilities.InputEvents.HardwareEvents
{
    public class KeyboardEventArgs
    {
        public Keys Key { get { return (Keys)VirtualKey; } }
        public int VirtualKey { get; private set; }
        public int ScanKey { get; private set; }
        public bool Pressed { get; private set; }

        public KeyboardEventArgs(Keys key, int scanKey, bool pressed)
            : this((int)key, scanKey, pressed)
        { }

        public KeyboardEventArgs(int virtualKey, int scanKey, bool pressed)
        {
            this.VirtualKey = virtualKey;
            this.ScanKey = scanKey;
            this.Pressed = pressed;
        }

        public static KeyboardEventArgs Shift(bool pressed)
        {
            return new KeyboardEventArgs(160, 42, pressed);
        }
    }
}
