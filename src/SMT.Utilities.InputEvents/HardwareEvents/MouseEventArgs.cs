using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMT.Utilities.InputEvents.HardwareEvents
{
    public class MouseEventArgs
    {
        internal readonly int DiffX;
        internal readonly int DiffY;
        internal readonly uint Flags;
        internal readonly int Data;

        private MouseEventArgs(int diffX, int diffY, uint flags, int data)
        {
            this.DiffX = diffX;
            this.DiffY = diffY;
            this.Flags = flags;
            this.Data = data;
        }

        public static MouseEventArgs LeftDown()
        {
            return new MouseEventArgs(0, 0, MouseEventDefinitions.MOUSEEVENTF_LEFTDOWN, 0);
        }

        public static MouseEventArgs LeftUp()
        {
            return new MouseEventArgs(0, 0, MouseEventDefinitions.MOUSEEVENTF_LEFTUP, 0);
        }

        public static MouseEventArgs RightDown()
        {
            return new MouseEventArgs(0, 0, MouseEventDefinitions.MOUSEEVENTF_RIGHTDOWN, 0);
        }

        public static MouseEventArgs RightUp()
        {
            return new MouseEventArgs(0, 0, MouseEventDefinitions.MOUSEEVENTF_RIGHTUP, 0);
        }

        public static MouseEventArgs ScrollUp()
        {
            return new MouseEventArgs(0, 0, MouseEventDefinitions.MOUSEEVENTF_WHEEL, 120);
        }

        public static MouseEventArgs ScrollDown()
        {
            return new MouseEventArgs(0, 0, MouseEventDefinitions.MOUSEEVENTF_WHEEL, -120);
        }

        public static MouseEventArgs Move(int diffX, int diffY)
        {
            return new MouseEventArgs(diffX, diffY, MouseEventDefinitions.MOUSEEVENTF_MOVE, 0);
        }

        public static MouseEventArgs Set(float x, float y)
        {
            var xDiff = (int)(65535f * x);
            var yDiff = (int)(65535f * y);

            return new MouseEventArgs(xDiff, yDiff, MouseEventDefinitions.MOUSEEVENTF_ABSOLUTE | MouseEventDefinitions.MOUSEEVENTF_MOVE, 0);
        }
    }
}
