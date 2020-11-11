using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    public delegate void CancelEventHandler(object sender, CancelEventArgs e);
    public class CancelEventArgs
    {
        public bool Cancelled { get; set; }

        public CancelEventArgs(bool cancelled = false)
        {
            Cancelled = cancelled;
        }
    }
}
