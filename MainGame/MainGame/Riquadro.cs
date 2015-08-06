using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    public class Riquadro
    {
      
        private Rectangle _superficie;
        public Rectangle Superficie { get { return _superficie; } }
        private float _larghRel, _altRel;

        protected Riquadro (Point VerticeAltoADestra, float LarghRelativa, float AltezzaRelativa, int LarghSchermo, int AltezzaSchermo ){
            _larghRel = LarghRelativa;
            _altRel = AltezzaRelativa;
            MathHelper.Clamp(_larghRel, 0, 1);
            MathHelper.Clamp(_altRel, 0, 1);
            float w = LarghRelativa * LarghSchermo;
            float h = AltezzaRelativa * AltezzaSchermo;
            _superficie = new Rectangle(VerticeAltoADestra.X, VerticeAltoADestra.Y, (int)w, (int)h);
            Game1.Ridimensionamento += GestisciRidimensionamento;
        }

        //Costruttore per un riquadro a tutto schermo
        protected Riquadro ()
        {
            _larghRel = _altRel = 1f;
            _superficie = new Rectangle(new Point(0, 0), new Point(GraphicsDeviceManager.DefaultBackBufferWidth, GraphicsDeviceManager.DefaultBackBufferHeight));
            Game1.Ridimensionamento += GestisciRidimensionamento;
        }

        

        /// <summary>
        /// Costruttore per un Riquadro il cui bordo inferiore coincide con il bordo dello schermo
        /// </summary>
        protected Riquadro(Point VerticeAltoADestra, float LarghRelativa, int LarghSchermo, int AltezzaSchermo)
        {
            _larghRel = LarghRelativa;
            _altRel = (AltezzaSchermo - VerticeAltoADestra.Y)/(float) AltezzaSchermo;
            MathHelper.Clamp(_larghRel, 0f, 1f);
            MathHelper.Clamp(_altRel, 0f, 1f);

            float w = _larghRel * LarghSchermo;
            float h = _altRel * AltezzaSchermo;
            _superficie = new Rectangle(VerticeAltoADestra.X, VerticeAltoADestra.Y, (int)w, (int)h);
            Game1.Ridimensionamento += GestisciRidimensionamento;
        }

        public bool Compreso (int x, int y)
        {
            return _superficie.Contains(x, y);
        }
        public bool Compreso(Point x)
        {
            return _superficie.Contains(x);
        }

        /// <summary>
        /// Event Handler che viene chiamato in caso di ridimensionamento della finestra di gioco
        /// e che forza il ricalcolo delle dimensioni del riquadro in pixel
        /// </summary>
        /// <param name="args"></param>
        protected virtual void GestisciRidimensionamento (object sender, ResizeEvntArgs args)
        {
            _superficie.Height = (int)(_altRel * args.newScreenHeight);
            _superficie.Width = (int)(_larghRel * args.newScreenHeight);
        }

        protected virtual void ClickSinistro (object sender, MouseEvntArgs args)
        {
            // gestione click sinistro standard
        }


    }
}
