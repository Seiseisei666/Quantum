using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    public class Riquadro
    {
      
        private Rectangle _superficie; // rettangolo che rappresenta il Riquadro
        /// <summary>
    /// Offset (in pixel) del Riquadro
    /// </summary>
        public Point Offset { get { return _superficie.Location; } }
        /// <summary>
        /// dimensione assoluta(in pixel) del Riquadro
        /// </summary>
        public int Larghezza { get { return _superficie.Width; } }
        /// <summary>
        /// dimensione assoluta(in pixel) del Riquadro
        /// </summary>
        public int Altezza { get { return _superficie.Height; } }

        private float _larghRel, _altRel;

        protected Riquadro (float xRel, float yRel, float LarghRelativa, float AltezzaRelativa, int LarghSchermo, int AltezzaSchermo ){
            float ofsX = MathHelper.Clamp(xRel, 0f, 1f);
            float ofsY = MathHelper.Clamp(yRel, 0f, 1f);
            _larghRel = MathHelper.Clamp(LarghRelativa, 0f, 1f);
            _altRel = MathHelper.Clamp(AltezzaRelativa, 0f, 1f);

            float w = (LarghRelativa - xRel) * LarghSchermo;
            float h = (AltezzaRelativa- yRel) * AltezzaSchermo;


            _superficie = new Rectangle((int)(xRel * LarghSchermo), (int)(yRel * AltezzaSchermo), (int)w, (int)h);
            Game1.Ridimensionamento += GestisciRidimensionamento;
        }

    
        /// <summary>
        /// Ritorna true se il punto è compreso nella superficie del riquadro
        /// </summary>
        public bool Compreso (int x, int y) {return _superficie.Contains(x, y);}

        /// <summary>
        /// Ritorna true se il punto è compreso nella superficie del riquadro
        /// </summary>
        public bool Compreso (Point x)  {return _superficie.Contains(x);}

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
