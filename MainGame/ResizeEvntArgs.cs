using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantum_Game
{
    public class ResizeEvntArgs : System.EventArgs
    {
        public int newScreenWidth, newScreenHeight;

        public ResizeEvntArgs (int w, int h)
        {
            newScreenWidth = w;
            newScreenHeight = h;
        }


    }
}
