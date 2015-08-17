using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    public sealed class GUI
    {
        public GUI (Quantum game, Texture2D texture)
        {
            _game = game;
            _texture = texture;
            _elementi = new List<Riquadro>();
            _spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }
    


        // Proprietà pubbliche
        public bottone BottonePremuto   // Da controllare ogni update, fornisce il bottone che è stato premuto
        {
            get
            {
                var bottoni = _elementi.OfType<IBottone>();
                foreach (var b in bottoni)
                    if (b.Check)
                        return b.TipoBottone;

                return bottone.nessuno;
            }
        }
        public Tabellone tabellone
        {
            get
            {
                return _elementi.OfType<Tabellone>().First();
            }
        }

        public SpriteFont Font { set { font = value; } }

        // METODI IMPORTANTI
        public void Draw (SpriteBatch spriteBatch)
        {
            var bottoni = _elementi.OfType<Bottone>();
            foreach (var b in bottoni)
            {
                b.Draw(spriteBatch, _texture);

            }
        }

       


        // Metodi scemi

        public void AddElement (Riquadro riquadro)
        {
            MouseInput mouseInput = (MouseInput)_game.GetGameObject(typeof(MouseInput));
            riquadro.AssociaEvento(mouseInput, TipoEventoMouse.ClkSin);
            Bottone bot = riquadro as Bottone;
            if (bot != null)
            {
                bot.Font = this.font;
                _elementi.Add(bot);
            }
            else
                _elementi.Add(riquadro);
        }

        // Campi privati
        private List<Riquadro> _elementi;
        private Quantum _game;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private SpriteFont font;

        // Proprietà private
        private int _numElementi { get { return _elementi.Count; } }


    }
}
