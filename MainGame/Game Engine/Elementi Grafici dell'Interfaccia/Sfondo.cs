using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Interfaccia
{
    /// <summary>
    /// Classe ipotetica per gestire lo sfondo (soprattuto in caso di future animazioni simpatiche)
    /// </summary>
    public class Sfondo: DrawableGameComponent
    {
        public Sfondo (Game game): base (game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            var schermo = _game.GraphicsDevice.Viewport;
            _w = schermo.Width;
            _h = schermo.Height;

            spriteBatch = _game.Services.GetService<SpriteBatch>();
            _texture = _game.Services.GetService<GuiManager>().SpriteSheet;

            _n = (int)_w / 400; _m = (int)_h / 100; _n++; _m++;

        }

        public override void Update(GameTime gameTime)
        {
            // Qui possiamo gestire le animazioni dello sfondo
            // TODO: fare un bel cielo stellato che si muove un po' a caso molto lentamente!

        }

        public void Draw ()
        {
            // Disegno minimale con le stelle in ordine
            for (int righe = 0; righe< _m; righe++)
            {
                for (int col = 0; col < _n; col++)
                {

                    spriteBatch.Draw(_texture, new Rectangle(col * 400 , righe * 100 , 400, 100), new Rectangle(0, 200, 400, 100), Color.White);
                }
            }
        }

        Game _game;
        SpriteBatch spriteBatch;
        Texture2D _texture;
        float _w, _h; // larghezza e altezza schermo
        int _n, _m; // tile stellati contenuti nelle due dimensioni dello schermo
    }
}
