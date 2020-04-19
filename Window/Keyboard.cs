using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTK.Input;

namespace Minecraft.MainWindow
{
    internal class WindowsAPI
    {
        /// <summary>
        /// The GetForegroundWindow function returns a handle to the foreground window.
        /// </summary>
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
          IntPtr hProcess,
          IntPtr lpBaseAddress,
          [Out()] byte[] lpBuffer,
          int dwSize,
          out int lpNumberOfBytesRead
         );

        public static void SwitchWindow(IntPtr windowHandle)
        {
            if (GetForegroundWindow() == windowHandle)
                return;

            IntPtr foregroundWindowHandle = GetForegroundWindow();
            uint currentThreadId = GetCurrentThreadId();
            uint temp;
            uint foregroundThreadId = GetWindowThreadProcessId(foregroundWindowHandle, out temp);
            AttachThreadInput(currentThreadId, foregroundThreadId, true);
            SetForegroundWindow(windowHandle);
            AttachThreadInput(currentThreadId, foregroundThreadId, false);

            while (GetForegroundWindow() != windowHandle)
            {
            }
        }

        /// <param name="hwndParent"></param>
        /// <param name="hwndChildAfter"></param>
        /// <param name="lpszClass"></param>
        /// <param name="lpszWindow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        /// <param name="ch"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern byte VkKeyScan(char ch);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        /// <param name="name"></param>
        /// <returns></returns>
        public static IntPtr FindWindow(string name)
        {
            Process[] procs = Process.GetProcesses();

            foreach (Process proc in procs)
            {
                if (proc.MainWindowTitle == name)
                {
                    return proc.MainWindowHandle;
                }
            }

            return IntPtr.Zero;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static int MakeLong(int low, int high)
        {
            return (high << 16) | (low & 0xffff);
        }

        [DllImport("User32.dll")]
        public static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] input, int structSize);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();

        public const int INPUT_MOUSE = 0;
        public const int INPUT_KEYBOARD = 1;
        public const int INPUT_HARDWARE = 2;
        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;
        public const uint KEYEVENTF_UNICODE = 0x0004;
        public const uint KEYEVENTF_SCANCODE = 0x0008;
        public const uint XBUTTON1 = 0x0001;
        public const uint XBUTTON2 = 0x0002;
        public const uint MOUSEEVENTF_MOVE = 0x0001;
        public const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const uint MOUSEEVENTF_LEFTUP = 0x0004;
        public const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        public const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        public const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        public const uint MOUSEEVENTF_XDOWN = 0x0080;
        public const uint MOUSEEVENTF_XUP = 0x0100;
        public const uint MOUSEEVENTF_WHEEL = 0x0800;
        public const uint MOUSEEVENTF_VIRTUALDESK = 0x4000;
        public const uint MOUSEEVENTF_ABSOLUTE = 0x8000;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct MOUSEINPUT
    {
        int dx;
        int dy;
        uint mouseData;
        uint dwFlags;
        uint time;
        IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct KEYBDINPUT
    {
        public ushort wVk;
        public ushort wScan;
        public uint dwFlags;
        public uint time;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct HARDWAREINPUT
    {
        uint uMsg;
        ushort wParamL;
        ushort wParamH;
    }

    [StructLayout(LayoutKind.Explicit)]
    internal struct INPUT
    {
        [FieldOffset(0)]
        public int type;
        [FieldOffset(4)] //*
        public MOUSEINPUT mi;
        [FieldOffset(4)] //*
        public KEYBDINPUT ki;
        [FieldOffset(4)] //*
        public HARDWAREINPUT hi;
    }

    /// <summary>
    /// A API for the keyboard
    /// </summary>
    public class Keyboard
    {
        [DllImport("User32.dll")]
        private static extern uint SendInput(uint numberOfInputs, [MarshalAs(UnmanagedType.LPArray, SizeConst = 1)] INPUT[] input, int structSize);

        private const int KeyDown = 1;
        private const int KeyUp = 2;
        private const uint testKey = 0x09;


        public static void SimulateKeyPress(ushort scanCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.dwFlags = 8;
            inputs[0].ki.wScan = (scanCode);

            uint intReturn = WindowsAPI.SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }

        public static void SimulateKeyRelease(ushort scanCode)
        {
            INPUT[] inputs = new INPUT[1];
            inputs[0].type = WindowsAPI.INPUT_KEYBOARD;
            inputs[0].ki.dwFlags = WindowsAPI.KEYEVENTF_KEYUP;
            inputs[0].ki.wScan = scanCode;
            uint intReturn = WindowsAPI.SendInput(1, inputs, Marshal.SizeOf(inputs[0]));
            if (intReturn != 1)
            {
                throw new Exception("Could not send key: " + scanCode);
            }
        }

        /// <summary>
        /// Fires when a key went down
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyPressed;
        /// <summary>
        /// Fires when a key went up
        /// </summary>
        public event EventHandler<KeyboardEventArgs> KeyReleased;

        private List<Key> pressedKeys = new List<Key>();

        /// <summary>
        /// Registers a key press. NOTE: This will have no actual key press. To simulate one, use <see cref="SimulateKeyPress(IntPtr, Key)"/>
        /// </summary>
        /// <param name="key">The key that went down</param>
        public void RegisterPressKey(Key key)
        {
            if (!pressedKeys.Contains(key))
            {
                if (!pressedKeys.Contains(key)) pressedKeys.Add(key);
                KeyPressed?.Invoke(this, new KeyboardEventArgs(key));
            }
        }
        /// <summary>
        /// Registers a key release. NOTE: This will have no actual key release. To simulate one, use <see cref="SimulateKeyRelease(IntPtr, Key)"/>
        /// </summary>
        /// <param name="key">The key that went up</param>
        public void RegisterReleaseKey(Key key)
        {
            if (pressedKeys.Contains(key)) pressedKeys.Remove(key);
            KeyReleased?.Invoke(this, new KeyboardEventArgs(key));
        }

        /// <summary>
        /// Checks if a key is being pressed
        /// </summary>
        /// <param name="key">The key that needs to be checked</param>
        /// <returns>The state of the key</returns>
        public bool IsKeyPressed(Key key)
        {
            return pressedKeys.Contains(key);
        }
    }
}