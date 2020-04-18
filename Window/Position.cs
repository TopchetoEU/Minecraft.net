using System;
using System.Drawing;

namespace Minecraft.MainWindow
{
    public class Position
    {
        public static readonly Position Zero = new Position(0, 0);

        public event EventHandler XChanged;
        public event EventHandler YChanged;

        private int x = 0;
        private int y = 0;
        public int X
        {
            get => x;
            set
            {
                x = value;
                XChanged?.Invoke(this, new EventArgs());
            }
        }
        public int Y
        {
            get => y;
            set
            {
                y = value;
                YChanged?.Invoke(this, new EventArgs());
            }
        }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Position(Point point)
        {
            x = point.X;
            y = point.Y;
        }
    }
}