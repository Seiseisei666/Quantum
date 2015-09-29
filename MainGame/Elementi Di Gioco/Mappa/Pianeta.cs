using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Quantum_Game
{
    public class Pianeta : Tile
    {
        static Random rnd = new Random(); // TEMPORANEO!! ***************
        //costruttore
        public Pianeta(QuantumTile tipo)
        {
            _tipo = tipo;
            _colonizzazioni = new e_color[4];

            
            e_color prova = (e_color)rnd.Next(1, 5);
            for (int i = 0; i < _colonizzazioni.Length; i++)
            {
                // TODO:    1) ogni pianeta deve avere un numero diverso di mentine
                //          2) questa inizializzazione è provvisoria, genera una mappa con mentine un po' a caso
                _colonizzazioni[i] = prova;
            }
        }

        // PROPRIETA' PUBBLICHE

            //override di Tile
        public override bool Esistente { get { return true; } }
            // l'array delle mentine
        public e_color[] Colonizzazioni { get { return _colonizzazioni; } }

        // METODI PUBBLICI
            // Per controllare se il pianeta è colonizzabile
        public bool Colonizzabile(e_color colore)
        {
            return _colonizzazioni.Any(x => x == 0) && !_colonizzazioni.Any(x => x == colore);
        }
        /// <summary> Restituisce True se sul pianeta c'è una mentina del giocatore argomento</summary>
        public override bool PresenzaAlleata (Giocatore player)
        {
            return _colonizzazioni.Any(c => c == player.Colore);
        }
        /// <summary> Restituisce True se sul pianeta c'è una mentina del colore della nave</summary>
        public override bool PresenzaAlleata(Nave nave)
        {
            return _colonizzazioni.Any(c => c == nave.Colore);
        }
        /// <summary>Determina se il giocatore argomento può colonizzare il pianeta.</summary>
        public bool Colonizzabile(Giocatore player)
        {
            int punteggio = 0;
            e_color colore = player.Colore;
            bool spazio = _colonizzazioni.Any(x => x == 0) && !_colonizzazioni.Any(x => x == colore);

            foreach (Tile t in this.TileAdiacenti(false, false))
                if (t?.PresenzaAlleata(player) == true)
                {
                    var c = (Casella)t;
                    punteggio += c.Occupante.Pwr;
                }
            System.Diagnostics.Debug.WriteLine(punteggio);
            if (spazio && punteggio == (int)this.Tipo) return true;
            else return false;
        }
            // L'azione di colonizzazione vera e propria
        public bool Colonizza(Giocatore plyr)
        {
            if (plyr.PuòColonizzare && this.Colonizzabile(plyr.Colore))
            {
                // Da togliere il check sulla condizione Colonizzabile? (lo fa il programma prima?)
                for (int i = 0; i < _colonizzazioni.Length - 1; i++)
                {
                    if (_colonizzazioni[i] == 0)
                    {
                        _colonizzazioni[i] = plyr.Colore;
                        plyr.Azione(true);                 // Toglie 2 azioni al giocatore
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Disegna lo sprite corrispondente al pianeta
        /// </summary>
        public void Draw (SpriteBatch spriteBatch, Texture2D texture, Vector2 coordinate, Vector2 scala)
        {
            Rectangle source = Rectangle.Empty;
            switch (Tipo)
            {
                case QuantumTile.Pianeta7:
                    source = new Rectangle(0, 100, 100,100);
                    break;
                case QuantumTile.Pianeta8:
                    source = new Rectangle(100, 100,100, 100);
                    break;
                case QuantumTile.Pianeta9:
                    source = new Rectangle(200, 100, 100, 100);
                    break;
                case QuantumTile.Pianeta10:
                    source = new Rectangle(300, 100, 100, 100);
                    break;
            }

            spriteBatch.Draw(texture, coordinate-scala*50, sourceRectangle: source, scale: scala);
        }
        /// <summary>
        /// Disegna il numero che indica il valore del pianeta
        /// </summary>
        public void DrawValore (SpriteBatch spriteBatch, SpriteFont font, Vector2 coordinate)
        {
            string str = ((int)Tipo).ToString();
            Vector2 pos = coordinate - font.MeasureString(str) / 2;
            spriteBatch.DrawString(font, str, pos, Color.Gold, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
        }

        //campi propri
        private e_color[] _colonizzazioni;
      
    }
}
