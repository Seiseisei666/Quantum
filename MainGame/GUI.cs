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
        /// <summary>
        /// Nelle intenzioni GUI sarebbe l'oggetto master che gestisce tutta l'interfaccia grafica
        /// </summary>

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
        public bottone BottonePremuto { get { return _bottonePremuto; } }
        private bottone _bottonePremuto;
        public void Update ()   // Da controllare ogni update, fornisce il bottone che è stato premuto
        {
            foreach (var b in _bottoni)
                if (b.Check())
                {
                    _bottonePremuto = b.TipoBottone;
                    return;
                }
            _bottonePremuto = bottone.nessuno;
        }
        
        public SpriteFont Font { set { font = value; } }

        // METODI IMPORTANTI

        public void Initialize()
        {
        }
       

        public void Draw ()
        {
            // Da fare meglio
            foreach (var b in _bottoni)
            {
                b.Draw(_spriteBatch, _texture);
            }
        }

        // Metodi scemi, pecionate temporanee

        public void AddElement (Bottone bot)
        {

            bot.AssociaEvento(mouseInput, TipoEventoMouse.ClkSin);
            bot.AssociaEvento(mouseInput, TipoEventoMouse.Over);
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
            // PECIONATA TEMPORANEA
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
            //NON FUNZIONA, da sistemare
            _bottoni.RemoveAll(b => b.TipoBottone == bottone.Riconfigura || b.TipoBottone == bottone.UsaSpecial);
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

    }
}
