using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft
{
    /// <summary>
    /// All keys
    /// </summary>
    public enum Key
    {
        #pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        LeftButton = 0x01,
        RightButton,
        Cancel,
        MiddleButton,
        X1Button,
        X2Button,
        Backspace = 0x08,
        Tab,
        Clear = 0x0c,
        Enter,
        Shift = 0x10,
        Control,
        Menu,
        Pause,
        CapsLock,
        KanaMode,
        Hangul = 0x15,
        Hanguel = 0x15,
        IMEOn,
        JunjaMode,
        FinalMode,
        HanjaMode = 0x19,
        KanjiMode = 0x19,
        IMEOff,
        Escape,
        IMEConvert,
        IMENonconvert,
        IMEAccept,
        IMEModeChange,
        Space,
        PageUp,
        PageDown,
        End,
        Home,
        LeftArrow,
        UpArrow,
        RightArrow,
        DownArrow,
        Select,
        Print,
        Execute,
        Snapshot,
        Insert,
        Delete,
        Help,
        D0, D1, D2, D3, D4, D5, D6, D7, D8, D9,
        A = 0x41, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z,
        LeftWindows, RightWindows, AppsKey,
        Sleep = 0x5f,
        Num0, Num1, Num2, Num3, Num4, Num5, Num6, Num7, Num8, Num9,
        NumMultiply,
        NumAdd,
        NumSeperator,
        NumSubtract,
        NumDecimal,
        NumDivide,
        F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12, F13, F14, F15, F16, F17, F18, F19, F20, F21, F22, F23, F24,
        NumLock = 0x90,
        ScrollLock,
        LeftShift = 0xa0,
        RightShift,
        LeftControl,
        RightControl,
        LeftMenu,
        RightMenu,
        BrowserBack,
        BrowserForward,
        BrowserRefresh,
        BrowserStop,
        BrowserSearch,
        BrowserFavourites,
        BrowserHome,
        VolumeMute,
        VolumeDown,
        VolumeUp,
        NextTrack,
        PreviousTrack,
        StopMedia,
        PLayMedia,
        LaunchMail,
        SelectMedia,
        App1, App2,
        OemSemicolon = 0xba,
        OemPlus,
        OemComma,
        OemMinus,
        OemPeriod,
        OemSlash,
        OemBackquote,
        OemOpenBrackets = 0xdb,
        OemBackslash,
        OemCloseBrackets,
        OemQuote,
        Oem8,
        Oem102 = 0xe2,
        Proccess = 0xe5,
        Packet = 0xe7,
        Attn = 0xf6,
        CrSel,
        ExSel,
        EraseEOF,
        Play,
        Zoom,
        NoName,
        Pa1,
        OemClear
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }

    /// <summary>
    /// All methods involving <see cref="Key"/> and <see cref="OpenTK.Input.Key"/> enums
    /// </summary>
    public static class KeyExtensions
    {
        /// <summary>
        /// Converts <see cref="Key"/> to <see cref="OpenTK.Input.Key"/>
        /// </summary>
        /// <param name="key">The key to be converted</param>
        /// <returns>The equivalent <see cref="OpenTK.Input.Key"/> key</returns>
        public static OpenTK.Input.Key ToOpenTKKey(this Key key)
        {
            switch (key)
            {
                case Key.Backspace: return OpenTK.Input.Key.BackSpace;
                case Key.Tab: return OpenTK.Input.Key.Tab;
                case Key.Clear: return OpenTK.Input.Key.Clear;
                case Key.Enter: return OpenTK.Input.Key.Enter;
                case Key.Shift: return OpenTK.Input.Key.ShiftLeft;
                case Key.Control: return OpenTK.Input.Key.ControlLeft;
                case Key.Menu: return OpenTK.Input.Key.Menu;
                case Key.Pause: return OpenTK.Input.Key.Pause;
                case Key.CapsLock: return OpenTK.Input.Key.CapsLock;
                #region Unknowns
                case Key.LeftButton:
                case Key.RightButton:
                case Key.Cancel:
                case Key.MiddleButton:
                case Key.X1Button:
                case Key.X2Button:
                case Key.KanaMode:
                case Key.IMEOn:
                case Key.JunjaMode:
                case Key.FinalMode:
                case Key.HanjaMode:
                case Key.IMEOff:
                case Key.IMEConvert:
                case Key.IMENonconvert:
                case Key.IMEAccept:
                case Key.IMEModeChange:
                case Key.Execute:
                case Key.Snapshot:
                case Key.Select:
                case Key.Help:
                case Key.AppsKey:
                case Key.BrowserBack:
                case Key.BrowserForward:
                case Key.BrowserRefresh:
                case Key.BrowserStop:
                case Key.BrowserSearch:
                case Key.BrowserFavourites:
                case Key.BrowserHome:
                case Key.VolumeMute:
                case Key.VolumeDown:
                case Key.VolumeUp:
                case Key.NextTrack:
                case Key.PreviousTrack:
                case Key.StopMedia:
                case Key.PLayMedia:
                case Key.LaunchMail:
                case Key.SelectMedia:
                case Key.App1:
                case Key.App2:
                case Key.Proccess:
                case Key.Packet:
                case Key.Attn:
                case Key.CrSel:
                case Key.ExSel:
                case Key.EraseEOF:
                case Key.Play:
                case Key.Zoom:
                case Key.NoName:
                case Key.Pa1:
                case Key.OemClear: return OpenTK.Input.Key.Unknown;
                #endregion
                case Key.Escape: return OpenTK.Input.Key.Escape;
                case Key.Space: return OpenTK.Input.Key.Space;
                case Key.PageUp: return OpenTK.Input.Key.PageUp;
                case Key.PageDown: return OpenTK.Input.Key.PageDown;
                case Key.End: return OpenTK.Input.Key.End;
                case Key.Home: return OpenTK.Input.Key.Home;
                case Key.LeftArrow: return OpenTK.Input.Key.Left;
                case Key.UpArrow: return OpenTK.Input.Key.Up;
                case Key.RightArrow: return OpenTK.Input.Key.Right;
                case Key.DownArrow: return OpenTK.Input.Key.Down;
                case Key.Print: return OpenTK.Input.Key.PrintScreen;
                case Key.Insert: return OpenTK.Input.Key.Insert;
                case Key.Delete: return OpenTK.Input.Key.Delete;
                #region Numbers
                case Key.D0: return OpenTK.Input.Key.Number0;
                case Key.D1: return OpenTK.Input.Key.Number1;
                case Key.D2: return OpenTK.Input.Key.Number2;
                case Key.D3: return OpenTK.Input.Key.Number3;
                case Key.D4: return OpenTK.Input.Key.Number4;
                case Key.D5: return OpenTK.Input.Key.Number5;
                case Key.D6: return OpenTK.Input.Key.Number6;
                case Key.D7: return OpenTK.Input.Key.Number7;
                case Key.D8: return OpenTK.Input.Key.Number8;
                case Key.D9: return OpenTK.Input.Key.Number9;
                #endregion
                #region Letters
                case Key.A: return OpenTK.Input.Key.A;
                case Key.B: return OpenTK.Input.Key.B;
                case Key.C: return OpenTK.Input.Key.C;
                case Key.D: return OpenTK.Input.Key.D;
                case Key.E: return OpenTK.Input.Key.E;
                case Key.F: return OpenTK.Input.Key.F;
                case Key.G: return OpenTK.Input.Key.G;
                case Key.H: return OpenTK.Input.Key.H;
                case Key.I: return OpenTK.Input.Key.I;
                case Key.J: return OpenTK.Input.Key.J;
                case Key.K: return OpenTK.Input.Key.K;
                case Key.L: return OpenTK.Input.Key.L;
                case Key.M: return OpenTK.Input.Key.M;
                case Key.N: return OpenTK.Input.Key.N;
                case Key.O: return OpenTK.Input.Key.O;
                case Key.P: return OpenTK.Input.Key.P;
                case Key.Q: return OpenTK.Input.Key.Q;
                case Key.R: return OpenTK.Input.Key.R;
                case Key.S: return OpenTK.Input.Key.S;
                case Key.T: return OpenTK.Input.Key.T;
                case Key.U: return OpenTK.Input.Key.U;
                case Key.V: return OpenTK.Input.Key.V;
                case Key.W: return OpenTK.Input.Key.W;
                case Key.X: return OpenTK.Input.Key.X;
                case Key.Y: return OpenTK.Input.Key.Y;
                case Key.Z: return OpenTK.Input.Key.Z;
                #endregion
                case Key.LeftWindows: return OpenTK.Input.Key.WinLeft;
                case Key.RightWindows: return OpenTK.Input.Key.WinRight;
                case Key.Sleep: return OpenTK.Input.Key.Sleep;
                #region Numpad
                case Key.Num0: return OpenTK.Input.Key.Keypad0;
                case Key.Num1: return OpenTK.Input.Key.Keypad1;
                case Key.Num2: return OpenTK.Input.Key.Keypad2;
                case Key.Num3: return OpenTK.Input.Key.Keypad3;
                case Key.Num4: return OpenTK.Input.Key.Keypad4;
                case Key.Num5: return OpenTK.Input.Key.Keypad5;
                case Key.Num6: return OpenTK.Input.Key.Keypad6;
                case Key.Num7: return OpenTK.Input.Key.Keypad7;
                case Key.Num8: return OpenTK.Input.Key.Keypad8;
                case Key.Num9: return OpenTK.Input.Key.Keypad9;
                case Key.NumMultiply: return OpenTK.Input.Key.KeypadMultiply;
                case Key.NumAdd: return OpenTK.Input.Key.KeypadAdd;
                case Key.NumSubtract: return OpenTK.Input.Key.KeypadSubtract;
                case Key.NumDecimal: return OpenTK.Input.Key.KeypadDecimal;
                case Key.NumDivide: return OpenTK.Input.Key.KeypadDivide;
                case Key.NumSeperator: return OpenTK.Input.Key.KeypadEnter;
                case Key.NumLock: return OpenTK.Input.Key.NumLock;
                #endregion
                #region F-s
                case Key.F1: return OpenTK.Input.Key.F1;
                case Key.F2: return OpenTK.Input.Key.F2;
                case Key.F3: return OpenTK.Input.Key.F3;
                case Key.F4: return OpenTK.Input.Key.F4;
                case Key.F5: return OpenTK.Input.Key.F5;
                case Key.F6: return OpenTK.Input.Key.F6;
                case Key.F7: return OpenTK.Input.Key.F7;
                case Key.F8: return OpenTK.Input.Key.F8;
                case Key.F9: return OpenTK.Input.Key.F9;
                case Key.F10: return OpenTK.Input.Key.F10;
                case Key.F11: return OpenTK.Input.Key.F11;
                case Key.F12: return OpenTK.Input.Key.F12;
                case Key.F13: return OpenTK.Input.Key.F13;
                case Key.F14: return OpenTK.Input.Key.F14;
                case Key.F15: return OpenTK.Input.Key.F15;
                case Key.F16: return OpenTK.Input.Key.F16;
                case Key.F17: return OpenTK.Input.Key.F17;
                case Key.F18: return OpenTK.Input.Key.F18;
                case Key.F19: return OpenTK.Input.Key.F19;
                case Key.F20: return OpenTK.Input.Key.F20;
                case Key.F21: return OpenTK.Input.Key.F21;
                case Key.F22: return OpenTK.Input.Key.F22;
                case Key.F23: return OpenTK.Input.Key.F23;
                case Key.F24: return OpenTK.Input.Key.F24;
                #endregion
                case Key.ScrollLock: return OpenTK.Input.Key.ScrollLock;
                case Key.LeftShift: return OpenTK.Input.Key.ShiftLeft;
                case Key.RightShift: return OpenTK.Input.Key.ShiftRight;
                case Key.LeftControl: return OpenTK.Input.Key.ControlLeft;
                case Key.RightControl: return OpenTK.Input.Key.ControlRight;
                case Key.LeftMenu: return OpenTK.Input.Key.WinLeft;
                case Key.RightMenu: return OpenTK.Input.Key.WinRight;
                case Key.OemSemicolon: return OpenTK.Input.Key.Semicolon;
                case Key.OemPlus: return OpenTK.Input.Key.Plus;
                case Key.OemComma: return OpenTK.Input.Key.Comma;
                case Key.OemMinus: return OpenTK.Input.Key.Minus;
                case Key.OemPeriod: return OpenTK.Input.Key.Period;
                case Key.OemSlash: return OpenTK.Input.Key.Slash;
                case Key.OemBackquote: return OpenTK.Input.Key.Tilde;
                case Key.OemOpenBrackets: return OpenTK.Input.Key.BracketLeft;
                case Key.OemBackslash: return OpenTK.Input.Key.BackSlash;
                case Key.OemCloseBrackets: return OpenTK.Input.Key.BracketRight;
                case Key.OemQuote: return OpenTK.Input.Key.Quote;
                case Key.Oem8:
                case Key.Oem102: return OpenTK.Input.Key.Unknown;
                default: return OpenTK.Input.Key.NonUSBackSlash;
            }
        }

        /// <summary>
        /// Converts <see cref="OpenTK.Input.Key"/> to <see cref="Key"/>
        /// </summary>
        /// <param name="key">The key to be converted</param>
        /// <returns>The equivalent <see cref="Key"/> key</returns>
        public static Key ToKey(this OpenTK.Input.Key key)
        {
            switch (key)
            {
                case OpenTK.Input.Key.Unknown: return Key.NoName;
                case OpenTK.Input.Key.ShiftLeft: return Key.LeftShift;
                case OpenTK.Input.Key.ShiftRight: return Key.RightShift;
                case OpenTK.Input.Key.ControlLeft: return Key.LeftControl;
                case OpenTK.Input.Key.ControlRight: return Key.RightControl;
                case OpenTK.Input.Key.AltLeft: return Key.LeftMenu;
                case OpenTK.Input.Key.AltRight: return Key.RightArrow;
                case OpenTK.Input.Key.WinLeft: return Key.LeftWindows;
                case OpenTK.Input.Key.WinRight: return Key.RightWindows;
                case OpenTK.Input.Key.Menu: return Key.Menu;
                case OpenTK.Input.Key.F1: return Key.F1;
                case OpenTK.Input.Key.F2: return Key.F2;
                case OpenTK.Input.Key.F3: return Key.F3;
                case OpenTK.Input.Key.F4: return Key.F4;
                case OpenTK.Input.Key.F5: return Key.F5;
                case OpenTK.Input.Key.F6: return Key.F6;
                case OpenTK.Input.Key.F7: return Key.F7;
                case OpenTK.Input.Key.F8: return Key.F8;
                case OpenTK.Input.Key.F9: return Key.F9;
                case OpenTK.Input.Key.F10: return Key.F10;
                case OpenTK.Input.Key.F11: return Key.F11;
                case OpenTK.Input.Key.F12: return Key.F12;
                case OpenTK.Input.Key.F13: return Key.F13;
                case OpenTK.Input.Key.F14: return Key.F14;
                case OpenTK.Input.Key.F15: return Key.F15;
                case OpenTK.Input.Key.F16: return Key.F16;
                case OpenTK.Input.Key.F17: return Key.F17;
                case OpenTK.Input.Key.F18: return Key.F18;
                case OpenTK.Input.Key.F19: return Key.F19;
                case OpenTK.Input.Key.F20: return Key.F20;
                case OpenTK.Input.Key.F21: return Key.F21;
                case OpenTK.Input.Key.F22: return Key.F22;
                case OpenTK.Input.Key.F23: return Key.F23;
                case OpenTK.Input.Key.F24: return Key.F24;
                case OpenTK.Input.Key.F25:
                case OpenTK.Input.Key.F26:
                case OpenTK.Input.Key.F27:
                case OpenTK.Input.Key.F28:
                case OpenTK.Input.Key.F29:
                case OpenTK.Input.Key.F30:
                case OpenTK.Input.Key.F31:
                case OpenTK.Input.Key.F32:
                case OpenTK.Input.Key.F33:
                case OpenTK.Input.Key.F34:
                case OpenTK.Input.Key.F35: return Key.NoName;
                case OpenTK.Input.Key.Up: return Key.UpArrow;
                case OpenTK.Input.Key.Down: return Key.DownArrow;
                case OpenTK.Input.Key.Left: return Key.LeftArrow;
                case OpenTK.Input.Key.Right: return Key.RightArrow;
                case OpenTK.Input.Key.Enter: return Key.Enter;
                case OpenTK.Input.Key.Escape: return Key.Escape;
                case OpenTK.Input.Key.Space: return Key.Space;
                case OpenTK.Input.Key.Tab: return Key.Tab;
                case OpenTK.Input.Key.BackSpace: return Key.Backspace;
                case OpenTK.Input.Key.Insert: return Key.Insert;
                case OpenTK.Input.Key.Delete: return Key.Delete;
                case OpenTK.Input.Key.PageUp: return Key.PageUp;
                case OpenTK.Input.Key.PageDown: return Key.PageDown;
                case OpenTK.Input.Key.Home: return Key.Home;
                case OpenTK.Input.Key.End: return Key.End;
                case OpenTK.Input.Key.CapsLock: return Key.CapsLock;
                case OpenTK.Input.Key.ScrollLock: return Key.ScrollLock;
                case OpenTK.Input.Key.PrintScreen: return Key.Print;
                case OpenTK.Input.Key.Pause: return Key.Pause;
                case OpenTK.Input.Key.NumLock: return Key.NumLock;
                case OpenTK.Input.Key.Clear: return Key.Clear;
                case OpenTK.Input.Key.Sleep: return Key.Sleep;
                case OpenTK.Input.Key.Keypad0: return Key.Num0;
                case OpenTK.Input.Key.Keypad1: return Key.Num1;
                case OpenTK.Input.Key.Keypad2: return Key.Num2;
                case OpenTK.Input.Key.Keypad3: return Key.Num3;
                case OpenTK.Input.Key.Keypad4: return Key.Num4;
                case OpenTK.Input.Key.Keypad5: return Key.Num5;
                case OpenTK.Input.Key.Keypad6: return Key.Num6;
                case OpenTK.Input.Key.Keypad7: return Key.Num7;
                case OpenTK.Input.Key.Keypad8: return Key.Num8;
                case OpenTK.Input.Key.Keypad9: return Key.Num9;
                case OpenTK.Input.Key.KeypadDivide: return Key.NumDivide;
                case OpenTK.Input.Key.KeypadMultiply: return Key.NumMultiply;
                case OpenTK.Input.Key.KeypadSubtract: return Key.NumSubtract;
                case OpenTK.Input.Key.KeypadAdd: return Key.NumAdd;
                case OpenTK.Input.Key.KeypadDecimal: return Key.NumDecimal;
                case OpenTK.Input.Key.KeypadEnter: return Key.NumSeperator;
                case OpenTK.Input.Key.A: return Key.A;
                case OpenTK.Input.Key.B: return Key.B;
                case OpenTK.Input.Key.C: return Key.C;
                case OpenTK.Input.Key.D: return Key.D;
                case OpenTK.Input.Key.E: return Key.E;
                case OpenTK.Input.Key.F: return Key.F;
                case OpenTK.Input.Key.G: return Key.G;
                case OpenTK.Input.Key.H: return Key.H;
                case OpenTK.Input.Key.I: return Key.I;
                case OpenTK.Input.Key.J: return Key.J;
                case OpenTK.Input.Key.K: return Key.K;
                case OpenTK.Input.Key.L: return Key.L;
                case OpenTK.Input.Key.M: return Key.M;
                case OpenTK.Input.Key.N: return Key.N;
                case OpenTK.Input.Key.O: return Key.O;
                case OpenTK.Input.Key.P: return Key.P;
                case OpenTK.Input.Key.Q: return Key.Q;
                case OpenTK.Input.Key.R: return Key.R;
                case OpenTK.Input.Key.S: return Key.S;
                case OpenTK.Input.Key.T: return Key.T;
                case OpenTK.Input.Key.U: return Key.U;
                case OpenTK.Input.Key.V: return Key.V;
                case OpenTK.Input.Key.W: return Key.W;
                case OpenTK.Input.Key.X: return Key.X;
                case OpenTK.Input.Key.Y: return Key.Y;
                case OpenTK.Input.Key.Z: return Key.Z;
                case OpenTK.Input.Key.Number0: return Key.D0;
                case OpenTK.Input.Key.Number1: return Key.D1;
                case OpenTK.Input.Key.Number2: return Key.D2;
                case OpenTK.Input.Key.Number3: return Key.D3;
                case OpenTK.Input.Key.Number4: return Key.D4;
                case OpenTK.Input.Key.Number5: return Key.D5;
                case OpenTK.Input.Key.Number6: return Key.D6;
                case OpenTK.Input.Key.Number7: return Key.D7;
                case OpenTK.Input.Key.Number8: return Key.D8;
                case OpenTK.Input.Key.Number9: return Key.D9;
                case OpenTK.Input.Key.Tilde: return Key.OemBackquote;
                case OpenTK.Input.Key.Minus: return Key.OemMinus;
                case OpenTK.Input.Key.Plus: return Key.OemPlus;
                case OpenTK.Input.Key.BracketLeft: return Key.OemOpenBrackets;
                case OpenTK.Input.Key.BracketRight: return Key.OemCloseBrackets;
                case OpenTK.Input.Key.Semicolon: return Key.OemSemicolon;
                case OpenTK.Input.Key.Quote: return Key.OemQuote;
                case OpenTK.Input.Key.Comma: return Key.OemComma;
                case OpenTK.Input.Key.Period: return Key.OemPeriod;
                case OpenTK.Input.Key.Slash: return Key.OemSlash;
                case OpenTK.Input.Key.BackSlash: return Key.OemBackslash;
                case OpenTK.Input.Key.NonUSBackSlash: return Key.OemBackslash;
                case OpenTK.Input.Key.LastKey: return Key.NoName;
                case OpenTK.Input.Key.Command: return Key.NoName;
                default: return Key.NoName;
            }
        }
    }
}
