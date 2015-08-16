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
        public GUI (Game game, Texture2D texture)
        {
            _game = game;
            _texture = texture;
            _elementi = new List<Riquadro>();
         //   spriteBatch = (SpriteBatch)game.Services.GetService(typeof(SpriteBatch));
        }
    


        // Proprietà pubbliche
        public bottone BottonePremuto   // Da controllare ogni update, fornisce il bottone che è stato premuto
        {
            get
            {
                foreach (IBottone bot in _elementi)
                {
                    if (bot.Check)
                        return bot.TipoBottone;
                }
                return bottone.nessuno;
            }
        }
        public Tabellone tabellone
        {
            get
            {
                return _elementi.Find(tab => tab.GetType() == typeof(Tabellone)) as Tabellone;
            }
        }

        public SpriteFont Font { set { font = value; } }

        // METODI IMPORTANTI
        public void Draw (SpriteBatch spriteBatch)
        {
            foreach (Riquadro r in _elementi)
                r.Draw(spriteBatch, _texture);
        }

       


        // Metodi scemi

        public void AddElement (Riquadro riquadro)
        {
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
        private Game _game;
  //      private SpriteBatch spriteBatch;
        private Texture2D _texture;
        private SpriteFont font;

        // Proprietà private
        private int _numElementi { get { return _elementi.Count; } }


    }
}
