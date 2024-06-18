using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Utilities.InputEvents.HardwareEvents
{
    /*
     * MSDN documentation
     * 
     *  typedef struct tagKBDLLHOOKSTRUCT {
     *      DWORD     vkCode;
     *      DWORD     scanCode;
     *      DWORD     flags;
     *      DWORD     time;
     *      ULONG_PTR dwExtraInfo;
     *  } KBDLLHOOKSTRUCT, *PKBDLLHOOKSTRUCT, *LPKBDLLHOOKSTRUCT;
     */

    internal struct KeyboardHardwareEventData
    {
        public int vkCode;
        public int scanCode;
        public int flags;
        public uint time;
        public IntPtr dwExtraInfo;
    }
}
