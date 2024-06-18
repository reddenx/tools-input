using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SMT.Utilities.InputEvents.Interfaces;

namespace SMT.Utilities.InputEvents.HardwareEvents
{
    public class KeyboardEventListener : IKeyboardEventListener
    {
        private KeyboardEventHandler Handler;

        private IntPtr HookId = IntPtr.Zero;
        private const int WH_KEYBOARD_LL = 13;

        private HookHandlerDelegate HardwareEventHandler;
        private delegate IntPtr HookHandlerDelegate(int nCode, IntPtr wParam, IntPtr lParam);

        public bool IsListening { get; private set; }

        public KeyboardEventListener()
        {
            IsListening = false;
        }

        public void StartListening(KeyboardEventHandler handler)
        {
            if (!IsListening)
            {
                IsListening = true;
                this.Handler = handler;
                HardwareEventHandler = new HookHandlerDelegate(HardwareHookCallback);
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    HookId = SetWindowsHookEx(WH_KEYBOARD_LL, HardwareEventHandler, LoadLibrary("User32"), 0);
                }
            }
        }

        private IntPtr HardwareHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            var lParamObj = (KeyboardHardwareEventData)Marshal.PtrToStructure(lParam, typeof(KeyboardHardwareEventData));
            if (nCode >= 0 && lParamObj.dwExtraInfo == (IntPtr)0)
            {
                var result = Handler(new KeyboardEventArgs(lParamObj.vkCode, lParamObj.scanCode, (uint)wParam == 256));
                if (result.WasHandled)
                {
                    return (IntPtr)1;
                }
            }

            return CallNextHookEx(HookId, nCode, wParam, lParam);
        }

        public void StopListening()
        {
            if (IsListening)
            {
                UnhookWindowsHookEx(HookId);
                IsListening = false;
            }
        }

        public void Dispose()
        {
            StopListening();
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookHandlerDelegate lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);
    }
}
