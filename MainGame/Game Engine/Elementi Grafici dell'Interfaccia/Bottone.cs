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
            mouseover = false;
            Enabled = true;
        }
        public Bottone (Riquadro contenitore): base (contenitore) { _tipoBottone = bottone.nessuno; Enabled = true; }
        public Bottone (Riquadro contenitore, string caption = " "): this (contenitore)
        {
            this.caption = caption;
        }

        #endregion
        public Color Colore { get { return colSfondo; } set { colSfondo = value; } }
        public bool Enabled { get; set; }

        public override void CaricaContenuti(GuiManager gui)
        {
            texture = gui.Pennello;
            font = gui.Font;


            // Calcolo della grandezza e della posizione della scritta da stampare sul bottone
            var grandezzaStringa = font.MeasureString(Caption);
            posScritta = new Vector2
                (contenitore.Superficie.Location.X + (contenitore.Superficie.Width - grandezzaStringa.X) / 2, contenitore.Superficie.Location.Y + (contenitore.Superficie.Height - grandezzaStringa.Y) / 2);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Color color = colSfondo;
            if (_contatoreIllumin > 0)
            { color = Color.OrangeRed; _contatoreIllumin--; }

            if (!Enabled) color = Color.Gray;
            float bright = mouseover ? 0.6f : 1;
            //bordo
            spriteBatch.Draw(texture, new Vector2 (contenitore.Superficie.X, contenitore.Superficie.Y), scale: new Vector2 (contenitore.Superficie.Width, contenitore.Superficie.Height), color: colBordo);
            //sfondo
            spriteBatch.Draw(texture, new Rectangle(contenitore.Superficie.Location.X + 2, contenitore.Superficie.Location.Y + 2, contenitore.Superficie.Width -4, contenitore.Superficie.Height - 4), color*bright);
            //scritta
            spriteBatch.DrawString(font, Caption, posScritta, Enabled ? Color.Black : Color.DarkGray);
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
                _contatoreIllumin = FRAME_ILLUMINATO;
                Click?.Invoke(this, EventArgs.Empty);
                //return;
            }
        }

        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            if (Enabled && contenitore.Superficie.Contains(args.Posizione))
                mouseover = true;
            else mouseover = false;
        }
        #endregion

        public virtual event EventHandler Click;


        #region Campi
        protected Vector2 posScritta;
        protected Texture2D texture;
        protected SpriteFont font;
        protected Color colBordo = Color.White;
        protected Color colSfondo = Color.Gold;
        protected Color colMouseOver = Color.Azure;

        private readonly bottone _tipoBottone;
        protected bool mouseover;
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
