using SMT.Utilities.InputEvents.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace SMT.Utilities.InputEvents.HardwareEvents
{
    public class KeyboardEventRunner : IKeyboardEventRunner
    {
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        public KeyboardEventRunner()
        {

        }

        public void DoEvent(KeyboardEventArgs eventArgs)
        {
            if (eventArgs.Pressed)
            {
                keybd_event((byte)eventArgs.VirtualKey, (byte)eventArgs.ScanKey, KEYEVENTF_EXTENDEDKEY, 1);
            }
            else
            {
                keybd_event((byte)eventArgs.VirtualKey, (byte)eventArgs.ScanKey, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 1);
            }
        }

        [DllImport("user32.dll")]
        private static extern bool keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
    }
}
