using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Interfaccia
{
    public class Dummy
    {
        public Dummy (Riquadro contenitore, int c)
        {
            _contenitore = contenitore;
            
            switch (c)
            {
                case 0: colore = Color.Azure; break;
                case 1: colore = Color.Black; break;
                case 2: colore = Color.Lime; break;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, _contenitore.Superficie, colore*0f);
            _contenitore.Superficie.Inflate(-5, -5);
            spriteBatch.Draw(texture,_contenitore.Superficie, colore);
        }
            Riquadro _contenitore;
        Color colore;
  
    }
}
