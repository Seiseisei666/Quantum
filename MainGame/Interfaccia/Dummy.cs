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
            spriteBatch.Draw(texture, new Rectangle(_contenitore.Xabs, (int)_contenitore.Yabs, (int)_contenitore.Largh, (int)_contenitore.Alt), colore*0f);
            spriteBatch.Draw(texture, new Rectangle(_contenitore.Xabs + 5, (int)_contenitore.Yabs + 5, (int)_contenitore.Largh-10, (int)_contenitore.Alt-10), colore);
        }
            Riquadro _contenitore;
        Color colore;
  
    }
}
