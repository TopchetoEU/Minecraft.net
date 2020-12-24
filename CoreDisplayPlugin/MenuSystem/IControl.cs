using NetGL.GraphicsAPI;
using NetGL.WindowAPI;
using System;
using System.Collections.Generic;

namespace MinecraftNet.MenuSystem
{
    public struct Margin
    {
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public static implicit operator Margin(int num)
        {
            return new Margin(num);
        }

        public int Horizontal {
            get {
                if (Left == Right)
                    return Left;
                else
                    return -1;
            }
            set {
                Left = Right = value;
            }
        }
        public int Vertical {
            get {
                if (Top == Bottom)
                    return Top;
                else
                    return -1;
            }
            set {
                Top = Bottom = value;
            }
        }

        public int All {
            get {
                if (Left == Right && Right == Top && Top == Bottom)
                    return Left;
                else
                    return -1;
            }
            set {
                Left = Right = Top = Bottom = value;
            }
        }

        public int HorizontalSum => Left + Right;
        public int VerticallSum => Top + Bottom;

        public Margin(int top, int bottom, int left, int right)
        {
            Top = top;
            Bottom = bottom;
            Left = left;
            Right = right;
        }
        public Margin(int vertical, int horizontal)
        {
            Left = Right = horizontal;
            Top = Bottom = vertical;
        }
        public Margin(int all)
        {
            Left = Right = Top = Bottom = all;
        }
    }
    public interface IControl
    {
        Margin Margin { get; set; }
        Margin Padding { get; set; }
    }
}
