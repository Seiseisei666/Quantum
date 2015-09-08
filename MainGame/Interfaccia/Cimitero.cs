using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game.Interfaccia
{
    public class Cimitero: RiquadroGui
    {
        public Cimitero (int x, int y, int l, int a): base (x,y,l, a)
        {
            
        }

        public override void Inizializzazione(GuiManager gui)
        {
            base.Inizializzazione(gui);

            _texture = gui.SpriteSheet;
            _font = gui.Font;
        }

        public void InizioTurno (Giocatore giocatore)
        {
            _giocatore = giocatore;
            _rottami = new List<Nave>(giocatore.Rottami);
        }


       
        public override void Draw (SpriteBatch spriteBatch)
        {
            // TODO: Disegnare lo sfondo

            int i = 0;
            if (_rottami != null)
            foreach (Nave n in _rottami)
            {

                spriteBatch.Draw(_texture, new Rectangle(Posizione.X +  (LARGH_SPRITES + DISTANZA_X) * i, Posizione.Y, LARGH_SPRITES, LARGH_SPRITES), new Rectangle(300, 0, 100, 100), Color.White);
                spriteBatch.DrawString(_font, n.Tipo.ToString(), new Vector2(Posizione.X + (LARGH_SPRITES + DISTANZA_X) * i, Posizione.Y + LARGH_SPRITES), Color.White);

                i++;
            }

            // TODO: Disegnare i bordi
        }

        


        Giocatore _giocatore;
        List<Nave> _rottami;
        Texture2D _texture;
        SpriteFont _font;

        const int LARGH_SPRITES = 50;
        const int DISTANZA_X = 25;
    }
}
