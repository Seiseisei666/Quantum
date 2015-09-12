using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Interfaccia
{
    public class Riquadro
    {
        public Riquadro(Riquadro parent, float Xorigine, float Yorigine, float larghezza,float altezza )
        {
            _parent = parent;
            Xrel = Xorigine;
            Yrel = Yorigine;
            LarghRel = larghezza;
            AltRel = altezza;
            if (_parent != null)_parent.Ridimensionamento += ridimensiona;
            Calcolo();
        }
        public static int LarghezzaSchermo { private get; set; }
        public static int AltezzaSchermo { private get; set; }
        public event EventHandler Ridimensionamento;

        void ridimensiona(object sender, EventArgs e) { Calcolo(); Ridimensionamento?.Invoke(this, EventArgs.Empty); }

        void Calcolo()
        {
            if (_parent == null)

            {
                Xabs = Yabs = 0;
                Largh = LarghezzaSchermo;
                Alt = AltezzaSchermo;
            }
            else
            {
                Xabs = (int)(Xrel/100 * _parent.Xabs);
                Yabs = (int)(Yrel/100 * _parent.Yabs);
                Largh = (int)(LarghRel/100 * _parent.Largh);
                Alt = (int)(AltRel/100 * _parent.Alt);
            }
        }






        Riquadro _parent;
       
        public float Xrel, Yrel, LarghRel, AltRel;
        public int Xabs, Yabs, Largh, Alt; 
    }
}
