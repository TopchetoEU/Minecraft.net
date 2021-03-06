﻿namespace NetGL
{
    /// <summary>
    /// A simple os detector, dependant on build type. NOTE! Unsupported operating systems fallback to windows
    /// </summary>
    internal static class OSDetector
    {
        #region OS Detection
#if LINUX
        private const OS os = OS.Linux;
        internal const string GraphicsDLL = "LinGL.so";
#elif MACOS
        private const OS os = OS.Mac;
        internal const string GraphicsDLL = "MacGL.dylib";
#else
        private const OS os = OS.Windows;
        internal const string GraphicsDLL = "WinGL.dll";
#endif
        #endregion

        public static string GetOSString(OS os)
        {
            switch (os)
            {
                case OS.Linux: return "LINUX";
                case OS.Mac: return "MACOS";
                case OS.Windows:
                default: return "DEFAULT";
            }
        }
        public static OS CurrentOS => os;
    }
}
