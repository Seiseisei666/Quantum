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
            _game = (Quantum)game;
        }

        public override void Initialize()
        {
            var schermo = _game.GraphicsDevice.Viewport;
            _w = schermo.Width;
            _h = schermo.Height;
            _n = (int)_w / 400; _m = (int)_h / 100; _n++; _m++;

            spriteBatch = _game.Services.GetService<SpriteBatch>();
            if (_game.InGioco) _texture = _game.Services.GetService<GuiManager>().SpriteSheet;
            else _texture = _game.Content.Load<Texture2D>(@"img\schermataIniziale_sfondo_2.1");
        }

        public override void Update(GameTime gameTime)
        {
            // Qui possiamo gestire le animazioni dello sfondo
            // TODO: fare un bel cielo stellato che si muove un po' a caso molto lentamente!

        }

        public void Draw ()
        {

            if (_game.InGioco)
            {
                // Disegno minimale con le stelle in ordine
                for (int righe = 0; righe < _m; righe++)
                {
                    for (int col = 0; col < _n; col++)
                    {

                        spriteBatch.Draw(_texture, new Rectangle(col * 400, righe * 100, 400, 100), new Rectangle(0, 200, 400, 100), Color.White);
                    }
                }
            }
            else { spriteBatch.Draw(_texture, new Rectangle(0, 0, _game.Window.ClientBounds.Width, _game.Window.ClientBounds.Height), Color.White); }
        }

        Quantum _game;
        SpriteBatch spriteBatch;
        Texture2D _texture;
        float _w, _h; // larghezza e altezza schermo
        int _n, _m; // tile stellati contenuti nelle due dimensioni dello schermo
    }
}
