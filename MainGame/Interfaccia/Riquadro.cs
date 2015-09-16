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
                Xabs = (int)(_parent.Xabs + (Xrel * _parent.Largh / 100f)); //OK
                Yabs = (int)(_parent.Yabs + (Yrel * _parent.Alt / 100f));
                Largh = (int)(LarghRel * _parent.Largh / 100f);
                Alt = (int)(AltRel * _parent.Alt / 100f);
            }
        }

        Riquadro _parent;
       
        public float Xrel, Yrel, LarghRel, AltRel;
        public int Xabs, Yabs, Largh, Alt; 
    }
}
