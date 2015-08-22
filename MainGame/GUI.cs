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
            _bottoni = new List<Bottone>();
            _spriteBatch = game.Services.GetService<SpriteBatch>();
            mouseInput = game.Services.GetService<MouseInput>();
        }
    


        // Proprietà pubbliche
        public Tabellone tabellone { get { return _tabellone; } }
        public bottone BottonePremuto   // Da controllare ogni update, fornisce il bottone che è stato premuto
        {
            get
            {
                foreach (var b in _bottoni)

                    if (b.Check())
                        return b.TipoBottone;

                return bottone.nessuno;
            }
        }
        
        public SpriteFont Font { set { font = value; } }

        // METODI IMPORTANTI

        public void Initialize() { }
       

        public void Draw ()
        {
            foreach (var b in _bottoni)
            {
                b.Draw(_spriteBatch, _texture);
            }
        }




        // Metodi scemi

        public void AddElement (Bottone bot)
        {

            bot.AssociaEvento(mouseInput, TipoEventoMouse.ClkSin);
            bot.Font = this.font;
            _bottoni.Add(bot);
        }
        public void AddElement (Tabellone tab)
        {
            tab.AssociaEvento(mouseInput, TipoEventoMouse.ClkSin);
            tab.AssociaEvento(mouseInput, TipoEventoMouse.ClkDx);
            tab.AssociaEvento(mouseInput, TipoEventoMouse.Over);
            _tabellone = tab;
        }



        public void PopupMenu(MenuTendina menu)
        {
            if (menu.Elementi.Count == 0)
                throw new ArgumentException("Menu' a tendina vuoto");
            if (_menu != null)
                throw new DeviceNotResetException("menù a tendina già presente");

            foreach (Bottone b in menu.Elementi)
            {
                b.AssociaEvento(mouseInput, TipoEventoMouse.Over);
                AddElement(b);
            }
            _menu = menu;
        }
   
        public void ChiudiMenu ()
        {
            foreach (Bottone b in _menu.Elementi)
            {

                _bottoni.Remove(b);
                System.Diagnostics.Debug.WriteLine(_bottoni.IndexOf(b));
            }

            _menu = null;
        }

        // Campi privati
        
        private List<Bottone> _bottoni;
        private Game _game;
        private Tabellone _tabellone;
        private MouseInput mouseInput;
        private MenuTendina _menu;

        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private SpriteFont font;

        // Proprietà private
        private int _numElementi { get { return _bottoni.Count; } }

        const int LARGH_MENU = 84;
        const int ALT_MENU = 30;
        const int OFFSETy = 10;

    }
}
