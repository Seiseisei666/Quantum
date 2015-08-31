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

    public class Bottone : RiquadroGui
    {
        #region Costruttori
        /// <summary> Costruttore per un Bottone senza parent </summary>
        public Bottone(bottone TipoBottone, int xRel, int yRel, int larghRel, int altRel) : 
            this (TipoBottone, xRel, yRel, larghRel, altRel, null)
        { // chiama il costruttore seguente
        } 
        /// <summary> Costruttore per un Bottone con un oggetto parent </summary>
        public Bottone(bottone TipoBottone, int xRel, int yRel, int larghRel, int altRel, object parent) :
            base(xRel, yRel, larghRel, altRel, parent)
        {
            _tipoBottone = TipoBottone;
            _cliccato = _mouseover = false;
            _parent = parent;
        }
        /// <summary> Costruttore statico per la voce di un menù a tendina </summary>
        public static Bottone MenuEntry (int num, bottone tipoBottone, object parent)
        {
            return new Bottone(tipoBottone, OFFSETX,( ALT_BOT *num), LARGH_BOT, ALT_BOT, parent);
        }
        /// <summary> Costruttore statico per un Bottone di dimensioni standard </summary>
        public static Bottone Standard (bottone tipoBottone, int xRel, int yRel)
        {
            return new Bottone(tipoBottone, xRel, yRel, LARGH_BOT, ALT_BOT);
        }

        #endregion

        public override void Inizializzazione(GuiManager gui)
        {
            _texture = gui.Pennello;
            font = gui.Font;

            // RiquardoGui.Inizializzazione() calcola le misure assolute in pixel di questo oggetto
            // Chiamarlo è assolutamente indispensabile!!!
            base.Inizializzazione(gui);

            // Calcolo della grandezza e della posizione della scritta da stampare sul bottone
            var grandezzaStringa = font.MeasureString(Caption);
            posScritta = new Vector2
                (Posizione.X + (Larghezza - grandezzaStringa.X) / 2, Posizione.Y + (Altezza - grandezzaStringa.Y) / 2);
            
        }

        public override void Riposiziona(Point posizione)
        {
            base.Riposiziona(posizione);
            posScritta += new Vector2(posizione.X, posizione.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // FIXME: caricare la texture
            Color color = _colSfondo;
            if (_contatoreIllumin > 0)
            { color = Color.Azure; _contatoreIllumin--; }
            else if (_mouseover)
                color = _colMouseOver;

            //bordo
            spriteBatch.Draw(_texture, new Rectangle(Posizione.X, Posizione.Y, Larghezza, Altezza), _colBordo);
            //sfondo
            spriteBatch.Draw(_texture, new Rectangle(Posizione.X + 3, Posizione.Y + 3, Larghezza - 6, Altezza - 6), color);
            //scritta
            spriteBatch.DrawString(font, Caption, posScritta, Color.Black);
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
            if (Compreso(args.Posizione.X, args.Posizione.Y))
            {
                _cliccato = true;
                _contatoreIllumin = FRAME_ILLUMINATO;
                return;
            }
        }

        protected override void MouseOver(object sender, MouseEvntArgs args)
        {
            if (Compreso(args.Posizione))
                _mouseover = true;
            else _mouseover = false;
        }
        #endregion



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
        const int FRAME_ILLUMINATO = 8;
        // Costanti dei bottoni standard
        const int LARGH_BOT = 20;
        const int ALT_BOT = 10;
        const int OFFSETX = 4;
        const int OFFSETY = 3;
        #endregion
    }
}
