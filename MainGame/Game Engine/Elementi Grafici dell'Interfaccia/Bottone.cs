using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quantum_Game;

namespace Quantum_Game.Interfaccia
{
    public enum bottone : byte
    {
        nessuno,
        Passa,
        Ricerca,
        UsaSpecial,
        Riconfigura,
        Colonizza,
    }

    public class Bottone : ElementoGrafico
    {
        #region Costruttori

        public Bottone (bottone TipoBottone, Riquadro contenitore): base (contenitore)
        {
            _tipoBottone = TipoBottone;
            _cliccato = _mouseover = false;
            Enabled = true;
        }

        #endregion

        public override void CaricaContenuti(GuiManager gui)
        {
            _texture = gui.Pennello;
            font = gui.Font;


            // Calcolo della grandezza e della posizione della scritta da stampare sul bottone
            var grandezzaStringa = font.MeasureString(Caption);
            posScritta = new Vector2
                (contenitore.Superficie.Location.X + (contenitore.Superficie.Width - grandezzaStringa.X) / 2, contenitore.Superficie.Location.Y + (contenitore.Superficie.Height - grandezzaStringa.Y) / 2);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = _colSfondo;
            if (_contatoreIllumin > 0)
            { color = Color.OrangeRed; _contatoreIllumin--; }
            else if (_mouseover) color = _colMouseOver;

            if (!Enabled) color = Color.Gray;

            //bordo
            spriteBatch.Draw(_texture, new Rectangle(contenitore.Superficie.Location.X, contenitore.Superficie.Location.Y, contenitore.Superficie.Width, contenitore.Superficie.Height), _colBordo);
            //sfondo
            spriteBatch.Draw(_texture, new Rectangle(contenitore.Superficie.Location.X + 3, contenitore.Superficie.Location.Y + 3, contenitore.Superficie.Width - 6, contenitore.Superficie.Height - 6), color);
            //scritta
            spriteBatch.DrawString(font, Caption, posScritta, Enabled ? Color.Black : Color.DarkGray);
        }

        public bool Enabled { get; set; }

        // proprietà pubbliche
        public string Caption
        {
            get
            {
                switch (_tipoBottone)
                {
                    case bottone.Passa:
                        return "Passa il turno";
                    case bottone.Ricerca:
                        return "Aumenta Ricerca";
                    case bottone.UsaSpecial:
                        return "Usa abilita speciale";
                    case bottone.Riconfigura:
                        return "Riconfigura";
                    case bottone.Colonizza:
                        return "Colonizza il Pianeta";
                    default:
                        return "";
                }
            }
        }   

        public bottone TipoBottone { get { return _tipoBottone; } }
        /// <summary>True se il bottone è stato cliccato dopo l'ultimo Reset. </summary>
        public bool Cliccato { get { return _cliccato; } }
        /// <summary>Resetta il bottone</summary>
        public void Reset() { _cliccato = false;}

        #region MouseInput
        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {
            if (Enabled && contenitore.Superficie.Contains(args.Posizione.X, args.Posizione.Y))
            {
                _cliccato = true;
                _contatoreIllumin = FRAME_ILLUMINATO;
                Click?.Invoke(this, EventArgs.Empty);
                //return;
            }
        }

        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            if (Enabled && contenitore.Superficie.Contains(args.Posizione))
                _mouseover = true;
            else _mouseover = false;
        }
        #endregion


        public event EventHandler Click;

        #region Campi
        Vector2 posScritta;
        Texture2D _texture;
        SpriteFont font;
        Color _colBordo = Color.White;
        Color _colSfondo = Color.Gold;
        Color _colMouseOver = Color.Azure;

        private readonly bottone _tipoBottone;
        private bool _cliccato;
        private bool _mouseover;
        private int _contatoreIllumin;
        const int FRAME_ILLUMINATO = 10;
        // Costanti dei bottoni standard
        const int LARGH_BOT = 20;
        const int ALT_BOT = 10;
        const int OFFSETX = 4;
        const int OFFSETY = 3;
        #endregion
    }
}
