using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Quantum_Game
{
    public sealed class GUI: IGameComponent
    {
        public GUI (Game game, Texture2D texture)
        {
            _game = game;
            _texture = texture;
            _elementi = new List<Riquadro>();
            _spriteBatch = game.Services.GetService<SpriteBatch>();
            mouseInput = game.Services.GetService<MouseInput>();
        }
    


        // Proprietà pubbliche
        public Tabellone tabellone { get { return _tabellone; } }
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
        
        public SpriteFont Font { set { font = value; } }

        // METODI IMPORTANTI

        public void Initialize ()
        {

        }
        public void Draw ()
        {
            var bottoni = _elementi.OfType<Bottone>();
            foreach (var b in bottoni)
            {
                b.Draw(_spriteBatch, _texture);

            }
        }




        // Metodi scemi

        public void AddElement (Bottone bot)
        {

            bot.AssociaEvento(mouseInput, TipoEventoMouse.ClkSin);
            bot.AssociaEvento(mouseInput, TipoEventoMouse.Over);
            bot.Font = this.font;
            _elementi.Add(bot);
        }
        public void AddElement (Tabellone tab)
        {
            tab.AssociaEvento(mouseInput, TipoEventoMouse.ClkSin);
            tab.AssociaEvento(mouseInput, TipoEventoMouse.ClkDx);
            tab.AssociaEvento(mouseInput, TipoEventoMouse.Over);
            _tabellone = tab;
        }
            


        

        // Campi privati
        private List<Riquadro> _elementi;
        private Game _game;
        private Tabellone _tabellone;
        private MouseInput mouseInput;

        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private SpriteFont font;

        // Proprietà private
        private int _numElementi { get { return _elementi.Count; } }


    }
}
