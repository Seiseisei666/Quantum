using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Interfaccia
{
    public class Riquadro
    {
        public float Xrelativa { get { return _Xrelativa; } }
        public float Yrelativa { get { return Yrelativa; } }
        public float LarghRelativa { get { return _larghRelativa; } }
        public float AltRelativa { get { return _altRelativa; } }
        public Rectangle Superficie { get { return new Rectangle(xAbs, yAbs, larghezzaAbs, altezzaAbs); } }

        protected List<Riquadro> _figli;

        public Riquadro( Riquadro parent, float Xorigine, float Yorigine, float larghezza,float altezza)
        {
            _figli = new List<Riquadro>();

            _Xrelativa = Xorigine;
            _Yrelativa = Yorigine;
            _larghRelativa = larghezza;
            _altRelativa = altezza;

            if (parent != null) parent.iscriviFiglio(this);
        }

        private void iscriviFiglio (Riquadro figlio)
        {


            _figli.Add(figlio);


            figlio.calcolaDimensioniInPixel(xAbs, yAbs, larghezzaAbs, altezzaAbs);
        }

        protected void calcolaDimensioniInPixel(int x, int y, int l, int a)
        {
            xAbs = (int)(x + (_Xrelativa * l / 100f));
            yAbs = (int)(y + (_Yrelativa * a / 100f));
            larghezzaAbs = (int)(_larghRelativa * l / 100f);
            altezzaAbs = (int)(_altRelativa * a / 100f);
            
            foreach (var figlio in _figli) figlio.calcolaDimensioniInPixel(xAbs, yAbs, larghezzaAbs, altezzaAbs);
        }

        protected void Ridimensionamento(object s, EventArgs e)
        {
            foreach (Riquadro figlio in _figli)
                figlio.calcolaDimensioniInPixel(xAbs, yAbs, larghezzaAbs, altezzaAbs);
        }

        private float _Xrelativa, _Yrelativa, _larghRelativa, _altRelativa;
        protected int xAbs, yAbs, larghezzaAbs, altezzaAbs; 
    }


    public class Schermo: Riquadro
    {
        GameWindow _finestra;


        public Schermo (GameWindow finestra): base (null, 0, 0, 100, 100)
        {
            _finestra = finestra;
            xAbs = 0; yAbs = 0;
            larghezzaAbs = finestra.ClientBounds.Width;
            altezzaAbs = finestra.ClientBounds.Height;
            finestra.ClientSizeChanged += Ridimensionamento;
        }

       
    }
}
