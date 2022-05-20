using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;

namespace JohnPranzo
{
	public static class KeyboardSimulator
    {

        static InputSimulator input = new InputSimulator();

        public static void VolumeUp()
        {
            input.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VOLUME_UP);
        }
    }
}
