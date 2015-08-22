using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Quantum_Game
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

    public class Bottone: Riquadro, IBottone
    {
        // costruttore
       
        public Bottone(bottone TipoBottone, float xRel, float yRel, float larghRel, float altRel, int LarghSch, int AltSch) : base(xRel, yRel, larghRel, altRel, LarghSch, AltSch)
        {
            _tipoBottone = TipoBottone;
            _cliccato = false;
        }
        /// <summary>
        /// Bottone con riquardo di dimensioni assolute.
        /// Per i menu' a tendina
        /// </summary>
        public Bottone (bottone TipoBottone, int xAbs, int yAbs, int Largh, int Altezza) : 
            base(xAbs, yAbs, Largh,Altezza)
        {
            _tipoBottone = TipoBottone;
            _cliccato = false;
        }
      

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
                        return "Aumenta il livello di ricerca";
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
        }   // nome del bottone, in caso dovessimo scriverlo a mano
        public bottone TipoBottone { get { return _tipoBottone; } }
        public bool Check ()
        {
                if (_cliccato)
                {
                    _cliccato = false;
                    return true;
                }
                return false;
        }

        public SpriteFont Font { set { font = value; } }
        public Vector2 grandezzaStringa { get { return font.MeasureString(Caption); } }
        // Override di Riquadro
        protected override void ClickSinistro(object sender, MouseEvntArgs args)
        {
            if (Compreso(args.Posizione.X, args.Posizione.Y))
            {
                _cliccato = true;
                _contatoreIllumin = FRAME_ILLUMINATO;
            }
        }

        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            if (Compreso(args.Posizione))
                _mouseover = true;
            else _mouseover = false;
        }

        public override void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {

            Color color = Color.Gold;
            if (_contatoreIllumin > 0)
            { color = Color.Azure; _contatoreIllumin--; }
            else if (_mouseover)
                color = Color.Beige;
            
                //bordo
            spriteBatch.Draw(texture, new Rectangle(Offset.X, Offset.Y, Larghezza, Altezza), Color.White);
                //sfondo
            spriteBatch.Draw(texture, new Rectangle(Offset.X+3, Offset.Y+3, Larghezza-6, Altezza-6), color);

            spriteBatch.DrawString(font, this.Caption, new Vector2(Offset.X + 10, Offset.Y+ Altezza/2), Color.Black);
        }


        private bottone _tipoBottone;
        private bool _cliccato;
        private bool _mouseover;
        private SpriteFont font;
        private int _contatoreIllumin;
        const int FRAME_ILLUMINATO = 8;

    }
}
