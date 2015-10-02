using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game.Interfaccia
{
    public enum bottone : byte
    {
        nessuno,
        IniziaPartita,
        Passa,
        Ricerca,
        Opzioni,
        Credits,
        più,
        meno,
        Annulla
    }

    //TODO: rendere abstract bottone
    public class Bottone : ElementoGrafico
    {
        #region Costruttori

        public Bottone (bottone TipoBottone, Riquadro contenitore): base (contenitore)
        {
            _tipoBottone = TipoBottone;
            _cliccato = _mouseover = false;
            Enabled = true;
        }
        public Bottone (Riquadro contenitore): base (contenitore) { _tipoBottone = bottone.nessuno; Enabled = true; }
        public Bottone (Riquadro contenitore, string caption = "", bool quadrato = false): this (contenitore)
        {
            this.caption = caption;
            if (quadrato)
            {
                int alt = contenitore.Superficie.Height; int larg = contenitore.Superficie.Width;
                bool largo = contenitore.Superficie.Width >= contenitore.Superficie.Height;
                if (largo) larg = alt; else alt = larg;
                contenitore.ModificaSuperficie(larg, alt);
            }
        }

        #endregion
        public Color Colore { set { _colSfondo = value; } }
        public bool Enabled { private get; set; }
        public bool èQuadrato { private get; set; }
        public bool èCentrato { private get; set; }

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

            if (!Enabled) color = Color.Gray;

            //bordo
            spriteBatch.Draw(_texture, new Rectangle(contenitore.Superficie.Location.X, contenitore.Superficie.Location.Y, contenitore.Superficie.Width, contenitore.Superficie.Height), _colBordo);
            //sfondo
            spriteBatch.Draw(_texture, new Rectangle(contenitore.Superficie.Location.X + 2, contenitore.Superficie.Location.Y + 2, contenitore.Superficie.Width -4, contenitore.Superficie.Height - 4), color);
            //scritta
            spriteBatch.DrawString(font, Caption, posScritta, Enabled ? Color.Black : Color.DarkGray);
            // effetto mouseover
            if (_mouseover)
                spriteBatch.Draw(_texture, new Rectangle(contenitore.Superficie.Location.X + 2, contenitore.Superficie.Location.Y + 2, contenitore.Superficie.Width - 4, contenitore.Superficie.Height - 4), Color.Black*0.3f);
        }

        private readonly string caption;
        // proprietà pubbliche
        public string Caption
        {
            get
            {
                switch (_tipoBottone)
                {
                    case bottone.IniziaPartita:
                        return "Nuova Partita";
                    case bottone.Passa:
                        return "Passa il turno";
                    case bottone.Ricerca:
                        return "Aumenta Ricerca";
                    case bottone.Opzioni:
                        return "Opzioni";
                    case bottone.Credits:
                        return "Credits";
                    case bottone.più:
                        return "+";
                    case bottone.meno:
                        return "-";
                    case bottone.Annulla:
                        return "Annulla";
                   
                    default:
                        return caption;
                }
            }
        }   


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
        //Vector2 offset = Vector2.Zero;
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
